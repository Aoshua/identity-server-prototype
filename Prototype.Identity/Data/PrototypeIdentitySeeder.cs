using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Configuration;
using Prototype.Identity.Data.Models;

namespace Prototype.Identity.Data
{
    public static class PrototypeIdentitySeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app?.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null) throw new NullReferenceException(nameof(serviceScope));

            var context = serviceScope?.ServiceProvider.GetRequiredService<PrototypeIdentityDbContext>();
            if (context == null) throw new NullReferenceException(nameof(context));
            context.Database.Migrate();

            var idsrvrContext = serviceScope?.ServiceProvider?.GetRequiredService<ConfigurationDbContext>();
            if (idsrvrContext == null) throw new NullReferenceException(nameof(idsrvrContext));
            idsrvrContext.Database.Migrate();

            serviceScope?.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            SeedTenants(context);
            SeedUsers(context);
            SeedIdentityServer(idsrvrContext);

            // Just to be sure no connections are left open
            context.Dispose(); 
            idsrvrContext.Dispose();
        }

        private static void SeedTenants(PrototypeIdentityDbContext context)
        {
            if (!context.Tenants.Any())
            {
                context.Tenants.Add(new Tenant()
                {
                    TenancyName = "Default",
                    Name = "Default"
                });

                context.SaveChanges();
            }
        }

        private static void SeedUsers(PrototypeIdentityDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User()
                {
                    Id = "b3ca6398-51cf-4da3-93e9-7816e852f1e8",
                    ShouldChangePasswordOnNextLogin = false,
                    SignInTokenExpireTimeUtc = null,
                    SignInToken = null,
                    GoogleAuthenticatorKey = null,
                    TopMenuId = 1,
                    LeftMenuId = 2,
                    CreationTime = DateTime.Parse("4/8/2022 11:00:00 AM"),
                    CreatorUserId = null,
                    LastModificationTime = DateTime.Parse("4/8/2022 11:00:00 AM"),
                    LastModifiedUserId = null,
                    IsDeleted = false,
                    DeleterUserId = null,
                    DeletionTime = null,
                    AuthenticationSource = null,
                    UserName = "steve64",
                    TenantId = 1,
                    Email = "steveknobs@gmail.com",
                    Name = "Steve",
                    Surname = "Knobs",
                    PasswordHash = "AQAAAAEAACcQAAAAEDEfweU/9o0j7QHoGqFlWgm97D+ZdEm2iudpUFPiXLAkGhR8lXCodjM2zYVtJpOtwQ==", // 123Qwe@
                    EmailConfirmationCode = null,
                    PasswordResetCode = null,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "5GFU7YSQHSF56K7YVPP6WKX6ESVV73IS",
                    TwoFactorEnabled = false,
                    EmailConfirmed = true,
                    IsActive = true,
                    NormalizedUserName = "STEVE64",
                    NormalizedEmail = "STEVEKNOBS@GMAIL.COM",
                    ConcurrencyStamp = "dc7a3874-c244-4a30-a935-247d00a2ce4b"
                });

                context.SaveChanges();
            }
        }

        private static void SeedIdentityServer(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in IdentityServerConfiguration.Clients)
                    context.Clients.Add(client.ToEntity());

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerConfiguration.IdentityResources)
                    context.IdentityResources.Add(resource.ToEntity());

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in IdentityServerConfiguration.ApiScopes)
                    context.ApiScopes.Add(resource.ToEntity());

                context.SaveChanges();
            }
        }
    }
}
