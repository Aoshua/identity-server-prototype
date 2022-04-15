using IdentityServer4.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Prototype.Identity.Data.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Prototype.Identity.Configuration
{
    public class PrototypeProfileService<TUser> : ProfileService<TUser> where TUser : User
    {
        private readonly UserManager<TUser> userManager;
        private readonly IUserClaimsPrincipalFactory<TUser> claimsFactory;

        public PrototypeProfileService( UserManager<TUser> userManager, IUserClaimsPrincipalFactory<TUser> claimsFactory)
            : base(userManager, claimsFactory)
        {
            this.userManager = userManager;
            this.claimsFactory = claimsFactory;
        }

        // This results in claims being added to the token, not just the response of conntect/userinfo
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            if (sub == null) throw new ArgumentNullException(nameof(sub));

            var user = await userManager.FindByIdAsync(sub);
            var principal = await claimsFactory.CreateAsync(user);

            //await base.GetProfileDataAsync(context); // do nothing for now

            var claims = principal.Claims.ToList();
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.ToString())).ToList(); // only return requested claims

            // ArcGIS claims
            claims.Add(new Claim("nickname", ""));
            claims.Add(new Claim("given_name", user.Name ?? ""));
            claims.Add(new Claim("middle_name", ""));
            claims.Add(new Claim("family_name", user.Surname ?? ""));
            context.IssuedClaims = claims;
        }
    }
}
