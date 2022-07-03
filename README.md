[![Website status](https://img.shields.io/website?label=Website%20status&url=https%3A%2F%2Ffireplace-of-despair.org%2F)](https://fireplace-of-despair.org)

[![Back/CodeQL](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_codeql.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_codeql.yml)
[![Back/Test](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_test.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_test.yml)
[![Coverage Status](https://coveralls.io/repos/github/ChiefNoir/ss-pbc/badge.svg?branch=master)](https://coveralls.io/github/ChiefNoir/ss-pbc?branch=master)

[![Front/CodeQL](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_codeql.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_codeql.yml)

[![Publish SSPBC](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_publish_public.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_publish_public.yml) 
[![Publish SSPBC.Admin](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_publish_admin.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_publish_admin.yml)
[![Publish SSPBC.Frontend](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_publish.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_publish.yml)

# SSPBC: Personal Business card
The personal web-site build almost at the enterprise level.
It features:
- Fully-fledged back-end: REST API with microservices.
- Relational and in-memory key–value databases.
- Fully-fledged front-end.
- Automated workflows with auto-tests, auto-codeQL.
- Automated publishing to the DockerHub: [pbc-public](https://hub.docker.com/r/ssproduction/pbc-public), [pbc-admin](https://hub.docker.com/r/ssproduction/pbc-admin), [pbc-front](https://hub.docker.com/r/ssproduction/pbc-front)


# System design
![system_design](https://user-images.githubusercontent.com/10946721/177055836-6bc27051-2bc2-48d1-a68b-968d8cda2719.png)

# Tech
## Front-end
- [React](https://reactjs.org/)
- [React Redux](https://react-redux.js.org/)
- [TypeScript](https://www.typescriptlang.org/)
- [MUI](https://mui.com/)
- [i18next](https://www.i18next.com/)
- _and other dependencies_

## Back-end
- [.Net Core 6](https://dotnet.microsoft.com/download)
- [EntityFrameworkCore](https://dotnet.microsoft.com/download)
- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/)
- [FluentMigrator](https://fluentmigrator.github.io/)
- [Jwt](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet)
- [xUnit](https://xunit.net/)
- _and other dependencies_

## Database
- [PostgreSQL](https://www.postgresql.org/)

## Cache
- [Redis](https://redis.io/)

## Reverse proxy
- [nginx](https://www.nginx.com/)

## CI/CD
- [GitHub Action](https://github.com/features/actions)
- [Docker](https://www.docker.com/)
