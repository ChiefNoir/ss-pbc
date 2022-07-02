# Back-end
REST API for the website.
Build on a [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) stack.
It uses  [PostgreSQL](https://www.postgresql.org/) as a long-term storage and [Redis](https://redis.io/) as a short-term storage


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
Most of the tests require a ready-to-use database. 
Most of the test cases are simulates user stories: look at projects, create projects, edit projects, etc, with fully operational database and caching systems.

# Requirements
- [Microsoft Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/community/)
- [dotnet 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

- [PostgreSQL](https://www.postgresql.org/)
- [Redis](https://redis.io/)
