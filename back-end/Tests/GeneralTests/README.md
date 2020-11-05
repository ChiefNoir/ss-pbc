# GeneralTests

General tests require ready-to-go PostgreSQL database on a localhost.

Credentials for database are listed in the [appsettings.test.json](https://github.com/ChiefNoir/BusinessCard/blob/master/back-end/Tests/GeneralTests/appsettings.test.json), ```ConnectionStrings:Default``` section.

General tests imitate general client-to-API behavior and use clean database for each test case, so tests must run without parallelization.
