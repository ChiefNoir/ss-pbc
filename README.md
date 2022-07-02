[![Website status](https://img.shields.io/website?label=Website%20status&url=https%3A%2F%2Ffireplace-of-despair.org%2F)](https://fireplace-of-despair.org)

[![Back/CodeQL](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_codeql.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_codeql.yml)
[![Back/Test](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_test.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/back-end_test.yml)
[![Coverage Status](https://coveralls.io/repos/github/ChiefNoir/ss-pbc/badge.svg?branch=master)](https://coveralls.io/github/ChiefNoir/ss-pbc?branch=master)

[![Front/CodeQL](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_codeql.yml/badge.svg)](https://github.com/ChiefNoir/ss-pbc/actions/workflows/front-end_codeql.yml)

# SSPBC
# Personal Business card
The personal web-site build almost at the enterprise level.
It features:
- Fully-fledged back-end: [REST API](https://en.wikipedia.org/wiki/Representational_state_transfer) with microservices.
- Relational and in-memory key–value databases.
- Fully-fledged front-end.
- Automated workflows with auto-tests, auto-codeQL and auto-publish docker images.

# System design
![design](https://user-images.githubusercontent.com/10946721/176768176-1bcb51c9-2245-477e-a671-745a89e5ff76.png)


# Tech
## Front-end
- [React](https://reactjs.org/)
- [React Redux](https://react-redux.js.org/)
- [TypeScript](https://www.typescriptlang.org/)
- [MUI](https://mui.com/)
- [i18next](https://www.i18next.com/)
- and other dependencies

## Back-end
- [.Net Core 6](https://dotnet.microsoft.com/download)
- [EntityFrameworkCore](https://dotnet.microsoft.com/download)
- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/)
- [FluentMigrator](https://fluentmigrator.github.io/)
- [Jwt](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet)
- [xUnit](https://xunit.net/)
- and other dependencies

## Database
- [PostgreSQL](https://www.postgresql.org/)

## Cache
- [Redis](https://redis.io/)

## CI/CD
- [GitHub Action](https://github.com/features/actions)
- [Docker](https://www.docker.com/)
