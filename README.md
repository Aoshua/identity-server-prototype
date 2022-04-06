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
