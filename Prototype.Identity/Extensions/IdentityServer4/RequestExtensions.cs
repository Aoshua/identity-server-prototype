// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Notice: Modified by Joshua Abbott

using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Prototype.Identity.Models;

namespace Prototype.Identity.Extensions.IdentityServer4
{
    public static class RequestExtensions
    {
        /// <summary>
        /// Checks if the redirect URI is for a native client
        /// </summary>
        public static bool IsNativeClient(this AuthorizationRequest context) =>
            !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
            && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);

        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUrl)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel(redirectUrl));
        }
    }
}
