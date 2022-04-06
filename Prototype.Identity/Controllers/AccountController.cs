// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Notice: Modified by Joshua Abbott

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prototype.Identity.Data.Models;
using Prototype.Identity.Extensions.ActionAttributes;
using Prototype.Identity.Extensions.IdentityServer4;
using Prototype.Identity.Models;

namespace Prototype.Identity.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IEventService events;
        private readonly IClientStore clientStore;
        private readonly TestUserStore users;

        public AccountController(
                IIdentityServerInteractionService interaction
                ,IClientStore clientStore // This may need to be replaced
                ,IAuthenticationSchemeProvider schemeProvider
                ,IEventService events
                ,TestUserStore users // TODO replace with real user store
            )
        {
            this.interaction = interaction;
            this.schemeProvider = schemeProvider;
            this.events = events;
            this.clientStore = clientStore;
            this.users = users;
        }
        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });

            return View(vm);
        }

        /// <summary>
        /// Handles the postback from the username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (button != "login") // If the users hits "cancel" button. TODO: this is dumb, rework
            {
                if (context != null)
                {
                    await interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                        return this.LoadingPage("Redirect", model.ReturnUrl);

                    return Redirect(model.ReturnUrl);
                }
                else return Redirect("~/"); // no valid context, go back to home page
            }

            if (ModelState.IsValid)
            {
                // Validate username & passsword (TODO: replace with actual user store)
                if (users.ValidateCredentials(model.Username, model.Password))
                {
                    var user = users.FindByUsername(model.Username);
                    await events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.Client.ClientId));

                    // Set explicit expiration if user has chosen "remember me"
                    // else used expiration configured by cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                        props = new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };

                    // Issue authentication cookie with subject ID and username
                    var issuer = new IdentityServerUser(user.SubjectId) { DisplayName = user.Username };

                    await HttpContext.SignInAsync(issuer, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        else
                            return Redirect(model.ReturnUrl);
                    }

                    // Request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("~/");
                    else // User might have clicked on a malicious link
                        throw new Exception("Invalid return URL.");
                }

                await events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            // Something went wrong, return model showing errors
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            // If the request to logout is properly authenticated, then
            // we don't need to show the prompt--we can simply log the user out directly
            if (vm.ShowLogoutPrompt == false)
                return await Logout(vm);

            return View(vm);
        }

        /// <summary>
        /// Handles the logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User.Identity?.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync(); // Delete local authentication cookie
                await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // Check if it is necessary to trigger sign out to an external IdP
            if (vm.TriggerExternalSignout)
            {
                // Construct a return URL so the external IdP can redirect back
                // to this provider after the user has logged out, allowing us
                // to complete sign-out processing.
                var url = Url.Action("Logout", new {logoutId = vm.LogoutId });

                // Trigger a redirect to the external provider for sign out
                return SignOut(new AuthenticationProperties() { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        #region Internal (should be moved

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);

            // External Login provider?
            if (context?.IdP != null && await schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel()
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context.LoginHint
                };

                if (!local)
                    vm.ExternalProviders = new[]
                    {
                        new ExternalProvider()
                        {
                            AuthenticationScheme = context.IdP
                        }
                    };

                return vm;
            }

            var schemes = await schemeProvider.GetAllSchemesAsync();

            var providers = schemes.Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider()
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                })
                .ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                        providers = providers.Where(x => client.IdentityProviderRestrictions.Contains(x.AuthenticationScheme)).ToList();
                }
            }

            return new LoginViewModel()
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint!,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User.Identity?.IsAuthenticated == false) // If user not authenticated, just show logged out page
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false) // It is safe to automatically sign out
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // Show the logout prompt. This prevents attacks where the user
            // is signed out by a malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var context = await interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel()
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = context?.PostLogoutRedirectUri,
                ClientName = String.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
                SignOutIframeUrl = context?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        // If there is not current logout context then we need to create one.
                        // This get the necessary info from the user to sign out and redirect
                        // to the external IdP for sign out.
                        if (vm.LogoutId == null)
                            vm.LogoutId = await interaction.CreateLogoutContextAsync();

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        #endregion
    }
}
