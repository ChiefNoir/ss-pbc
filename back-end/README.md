# Back-end
This is REST API for website.

## Projects
### Abstractions
Core abstractions and basic application layout. 

### Infrastructure
Interactions with long-term storage.

### Infrastructure.Cache
Interactions with short-term storage (like cache).

### Security
Security and supervision.
Token management, supervision, hashing, etc.
Any security-related things that do not require database / third-party services go here.

### SSPBC
The public API. Pretty generic and simple REST API. 

### SSPBC.Admin
The private API for the admin panel of the site.
Every method must have Authorize attribute at the declaration and session validation inside.


### Tests/GeneralTests
Unit and e2e tests.
Most of the tests requires a ready-to-use database. 
Most of the test cases are simulates user stories: look at projects, create project, edit project, etc, with fully operational database and caching systems.

# Requirements
- [Microsoft Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/community/)
- [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

- [PostgreSQL](https://www.postgresql.org/)
- [Redis](https://redis.io/)
