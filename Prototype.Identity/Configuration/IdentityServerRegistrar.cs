using IdentityServer4.Configuration;
using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Data.Models;
using System.Reflection;

namespace Prototype.Identity.Configuration
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot config)
        {
            // Specifying the migration assembly is necessary because the DbContext for 
            // the IdentityServer4 models is in a different assembly
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(options =>
                options.UserInteraction = new UserInteractionOptions()
                {
                    LoginUrl = "/Account/Login",
                    LogoutUrl = "/Account/Logout",
                    ErrorUrl = "/Error"
                })
                //.AddTestUsers(IdentityServerConfiguration.TestUsers)
                .AddAspNetIdentity<User>()
                .AddProfileService<PrototypeProfileService<User>>()
                // The intention of storing these configurations in the database
                // is to recover easily in the case of an app crash.
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(config.GetConnectionString("Default"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(config.GetConnectionString("Default"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddDeveloperSigningCredential(); // TODO: Production signing credentials
        }
    }
}
