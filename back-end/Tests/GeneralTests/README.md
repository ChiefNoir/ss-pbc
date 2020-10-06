# GeneralTests

General tests requires ready-to-go PostgreSQL database on a localhost.
Credentials for database are listed in the appsettings.test.json, ConnectionStrings:Test section.

General tests imitates general client-to-API behavior and uses clean database, so tests must run successively, without parallelization.