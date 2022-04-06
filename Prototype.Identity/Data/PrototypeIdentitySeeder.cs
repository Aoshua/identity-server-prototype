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

            SeedTenants(context);
            SeedUsers(context);

            context.Dispose(); // Just to be sure no connections are left open
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
                    ProfilePictureId = null,
                    ShouldChangePasswordOnNextLogin = false,
                    SignInTokenExpireTimeUtc = null,
                    SignInToken = null,
                    GoogleAuthenticatorKey = null,
                    TopMenuId = 1,
                    LeftMenuId = 2,
                    CreationTime = DateTime.Parse("4/6/2022 10:50:44 AM"),
                    CreatorUserId = null,
                    LastModificationTime = DateTime.Parse("4/6/2022 1:03:03 PM"),
                    LastModifiedUserId = null,
                    IsDeleted = false,
                    DeleterUserId = null,
                    DeletionTime = null,
                    AuthenticationSource = null,
                    UserName = "admin",
                    TenantId = 1,
                    Email = "default@defaulttenant.com",
                    Name = "admin",
                    Surname = "admin",
                    PasswordHash = "AQAAAAEAACcQAAAAEKYYP9iIhtl4F8pUSotG7YZgjhKjUVNr+tzN4/FpwZhUXdPTojk0JHTX9xRst1wttQ==",
                    EmailConfirmationCode = null,
                    PasswordResetCode = null,
                    LockoutEnd = null,
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "4db93525-dfb1-b18e-35a8-3a02e8c31769",
                    TwoFactorEnabled = false,
                    EmailConfirmed = true,
                    IsActive = true,
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "DEFAULT@DEFAULTTENANT.COM",
                    ConcurrencyStamp = "68335910-ec9d-477b-8dfb-71a7a9dfd3d1"
                });

                context.SaveChanges();
            }
        }
    }
}
