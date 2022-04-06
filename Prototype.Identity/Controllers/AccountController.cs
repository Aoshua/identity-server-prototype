using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
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
        private readonly IClientStore clientStore;

        public AccountController(
                IIdentityServerInteractionService interaction
                , IAuthenticationSchemeProvider schemeProvider
                , IClientStore clientStore
            )
        {
            this.interaction = interaction;
            this.schemeProvider = schemeProvider;
            this.clientStore = clientStore;
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

            //if (ModelState.IsValid)
            //{
            //    // Validate username & passsword
            //    if (users)
            //}
            return View(); // REMOVE
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);

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
    }
}
