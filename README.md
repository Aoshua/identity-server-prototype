# Identity Server Prototype
The purpose of the prototype is to stand up a proof of concept of an Identity Server/Provider built on IdentityServer4 that can provide authentication to a Vue client app.

## Setup
When running for the first time, set `MigrateAndSeed` to `true` in `appsettings.json`

## Multiple DB Contexts
There are 3 database contexts for this project:
1. The main app context: PrototypeIdentityDbContext
2. IdentityServer4's ConfigurationDbContext (used to configure clients, resources, scopes, etc.)
3. IdentityServer4's PersistedGrantDbContext (used for temporary operations data such as auth codes and refresh tokens)