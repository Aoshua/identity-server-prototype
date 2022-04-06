using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Configuration;

namespace Prototype.Identity.Data
{
    public static class IdentityServerSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app?.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null) throw new NullReferenceException(nameof(serviceScope));

            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope?.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            if (context == null) throw new NullReferenceException(nameof(context));

            context.Database.Migrate();
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
