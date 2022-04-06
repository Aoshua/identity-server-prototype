# Identity Server Prototype
The purpose of the prototype is to stand up a proof of concept of an Identity Server/Provider built on IdentityServer4 that can provide authentication to the following applications:
1. A .NET 6 Web API
2. A Vue Web App
3. ArcGIS/Esri

## Setup
1. Run migrations for the main db context: `cd Prototype.Identity` then `dotnet ef database update -c PrototypeIdentityDbContext`
2. Uncomment seeders in `Program.cs`: `IdentityServerSeeder.Seed(app);` and `PrototypeIdentitySeeder.Seed(app);`
3. Run the application
4. Shut down application then re-comment the lines in step 2. Then you're good to go.

## Multiple DB Contexts
There are 3 database contexts for this project:
1. The main app context: PrototypeIdentityDbContext
2. IdentityServer4's ConfigurationDbContext (used to configure clients, resources, scopes, etc.)
3. IdentityServer4's PersistedGrantDbContext (used for temporary operations data such as auth codes and refresh tokens)

If you do not wish to seed the database but only create the tables, then run the following commands:
`cd Prototype.Identity`
`dotnet ef database update -c PrototypeIdentityDbContext`
`dotnet ef database update -c ConfigurationDbContext`
`dotnet ef database update -c PersistedGrantDbContext`

## Todo:
☑️ Properly configure AspIdentity
☑️ Replace username & password validation logic
☑️ Move business logic out of controllers
