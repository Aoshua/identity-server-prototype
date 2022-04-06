There are 3 database contexts for this project:
1. The main app context: PrototypeIdentityDbContext
2. IdentityServer4's ConfigurationDbContext (used to configure clients, resources, scopes, etc.)
3. IdentityServer4's PersistedGrantDbContext (used for temporary operations data such as auth codes and refresh tokens)

To apply all migrations:
`cd Prototype.Identity`
`dotnet ef database update -c PrototypeIdentityDbContext`
`dotnet ef database update -c ConfigurationDbContext`
`dotnet ef database update -c PersistedGrantDbContext`