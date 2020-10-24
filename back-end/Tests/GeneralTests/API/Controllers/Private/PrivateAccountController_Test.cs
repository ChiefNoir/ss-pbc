using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Abstractions.Model.System;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateAccountController_Test
    {
        class SqlInsert : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new string[] { }
                };
                yield return new object[]
                {
                    new[]
                    {
                        "INSERT INTO account (login, password, salt, role) VALUES ('login', 'password', 'salt', 'role'); "
                    }
                };
                yield return new object[]
                {
                    new[]
                    {
                        "INSERT INTO account (login, password, salt, role) VALUES ('login', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (login, password, salt, role) VALUES ('login2', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (login, password, salt, role) VALUES ('login3', 'password', 'salt', 'role'); "
                    }
                };
            }
        }

        class SqlInsertWithResults : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); ",
                    new Account
                    {
                        Id = 3, Login = "login", Password = null, Role = "role", Version = 0
                    }
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login2', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (5, 'login3', 'password', 'salt', 'role'); "
                    ,
                    new Account
                    {
                        Id = 5, Login = "login3", Password = null, Role = "role", Version = 0
                    }
                };
            }
        }

        class ValidAdd : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "login",
                        Password = "Password",
                        Role = "admin"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "admin",
                        Password = "pswrd",
                        Role = "demo"
                    }
                };
            }
        }

        class Roles : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new[]
                    {
                        "admin",
                        "demo"
                    }

                };
            }
        }

        class InsertWithPagingAndResults : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    //(id:1) is for default sa@sa
                    "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password2', 'salt2', 'role2'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password3', 'salt3', 'role3'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login4', 'password4', 'salt4', 'role4'); "
                    ,
                    new Paging
                    {
                        Start = 1,
                        Length = 2
                    },
                    new []
                    {
                        new Account
                        {
                            Id = 2, Login = "login2", Password = null, Role = "role2", Version = 0
                        },
                        new Account
                        {
                            Id = 3, Login = "login3", Password = null, Role = "role3", Version = 0
                        },
                    }
                };
                yield return new object[]
                {
                    //(id:1) is for default sa@sa
                    "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password2', 'salt2', 'role2'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password3', 'salt3', 'role3'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login4', 'password4', 'salt4', 'role4'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (5, 'login5', 'password5', 'salt5', 'role5'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (6, 'login6', 'password6', 'salt6', 'role6'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (7, 'login7', 'password7', 'salt7', 'role7'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (8, 'login8', 'password8', 'salt8', 'role8'); "
                    ,
                    new Paging
                    {
                        Start = 5,
                        Length = 12
                    },
                    new []
                    {
                        new Account
                        {
                            Id = 6, Login = "login6", Password = null, Role = "role6", Version = 0
                        },
                        new Account
                        {
                            Id = 7, Login = "login7", Password = null, Role = "role7", Version = 0
                        },
                        new Account
                        {
                            Id = 8, Login = "login8", Password = null, Role = "role8", Version = 0
                        }
                    }
                };
            }
        }

        class InsertWithPagingAndBadToken : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    //(id:1) is for default sa@sa
                    "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password2', 'salt2', 'role2'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password3', 'salt3', 'role3'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login4', 'password4', 'salt4', 'role4'); "
                    ,
                    new Paging
                    {
                        Start = 1,
                        Length = 2
                    },
                    ""
                };
                yield return new object[]
                 {
                    //(id:1) is for default sa@sa
                    "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password2', 'salt2', 'role2'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password3', 'salt3', 'role3'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login4', 'password4', 'salt4', 'role4'); "
                    ,
                    new Paging
                    {
                        Start = 1,
                        Length = 2
                    },
                    null
                 };
                yield return new object[]
                {
                    //(id:1) is for default sa@sa
                    "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password2', 'salt2', 'role2'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password3', 'salt3', 'role3'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login4', 'password4', 'salt4', 'role4'); "
                    ,
                    new Paging
                    {
                        Start = 1,
                        Length = 2
                    },
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTk5Njk3MSwiZXhwIjoxNjAxOTk4NzcxLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.DCbppW8SqvL1QJS2BIO2qlplZv-UHqI2_NP_Za0KDzA"
                };
            }
        }

        class InvalidAdd : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "sa",
                        Password = "sa",
                        Role = "admin"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = null,
                        Password = "pswrd",
                        Role = "demo"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "admin",
                        Password = null,
                        Role = "demo"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "admin",
                        Password = "admin",
                        Role = null
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "admin",
                        Password = "admin",
                        Role = "something"
                    }
                };
            }
        }

        class InvalidUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account
                    {
                        Id = 10,
                        Login = "sa",
                        Password = "sa",
                        Role = "admin",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "sa",
                        Password = "sa",
                        Role = "admin",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "sa",
                        Password = "sa",
                        Role = "admin",
                        Version = 10
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "sa",
                        Password = "sa",
                        Role = null,
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = null,
                        Password = "sa",
                        Role = "admin",
                        Version = 10
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "sa",
                        Password = "sa",
                        Role = "wrong",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "login",
                        Password = "sa",
                        Role = "admin",
                        Version = 0
                    }
                };
            }
        }


        class InsertWithInvalidDelete : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null,
                    new Account
                    {
                        Id = 3, Login = "login", Password = null, Role = "role", Version = 0
                    }
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); ",
                    new Account
                    {
                        Id = null, Login = "login", Password = null, Role = "role", Version = 0
                    }
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); ",
                    new Account
                    {
                        Id = 3, Login = "login", Password = null, Role = "role", Version = 10
                    }
                };
            }
        }

        class InsertWithInvalidGet : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); ",
                    -1
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); ",
                    4
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login2', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (5, 'login3', 'password', 'salt', 'role'); "
                    ,
                    int.MaxValue
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login2', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (5, 'login3', 'password', 'salt', 'role'); "
                    ,
                    int.MinValue
                };
            }
        }

        // -- 

        [Theory]
        [ClassData(typeof(Roles))]
        internal void GetRolesAsync_Valid(string[] expectedRoles)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);

                    var response =
                    (
                        api.GetRoles() as JsonResult
                    ).Value as ExecutionResult<List<string>>;

                    GenericChecks.CheckSucceed(response);

                    Assert.Equal(expectedRoles.Length, response.Data.Count);

                    foreach (var item in expectedRoles)
                    {
                        var actural = response.Data.First(x => x == item);
                        Assert.Equal(item, actural);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        [Theory]
        [ClassData(typeof(SqlInsert))]
        internal async void CountAsync_Valid(string[] sql)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);

                    var response =
                    (
                        await api.CountAccountAsync() as JsonResult
                    ).Value as ExecutionResult<int>;
                    GenericChecks.CheckSucceed(response, true);

                    Assert.Equal(sql.Length + 1, response.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(ValidAdd))]
        internal async void AddAccountAsync_Valid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    var addResult =
                    (
                        await api.SaveAccountAsync(account) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(addResult);
                    Compare(addResult.Data, account);


                    var getResult =
                    (
                        await api.GetAccountAsync(addResult.Data.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(getResult);
                    Compare(getResult.Data, account);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidAdd))]
        internal async void AddAccountAsync_InValid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    //NOTE: creating a first valid account
                    var auth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await auth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var api = Storage.CreatePrivateController(context);
                    var addResult =
                    (
                        await api.SaveAccountAsync(account) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckFail(addResult);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        [Theory]
        [ClassData(typeof(SqlInsertWithResults))]
        internal async void DeleteAccountAsync_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);
                    var delResult =
                    (
                        await api.DeleteAccountAsync(account) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckSucceed(delResult);


                    var responseGet =
                    (
                        await api.GetAccountAsync(account.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckFail(responseGet);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InsertWithInvalidDelete))]
        internal async void DeleteAccountAsync_InValid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);
                    var delResult =
                    (
                        await api.DeleteAccountAsync(account) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckFail(delResult);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        [Theory]
        [ClassData(typeof(SqlInsertWithResults))]
        internal async void GetAccountAsync_Valid(string sql, Account expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);
                    var getResult =
                    (
                        await api.GetAccountAsync(expected.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(getResult);
                    Compare(getResult.Data, expected);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InsertWithInvalidGet))]
        internal async void GetAccountAsync_InValid(string sql, int id)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);
                    var getResult =
                    (
                        await api.GetAccountAsync(id) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckFail(getResult);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        [Fact]
        internal async void UpdateAcountAsync_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var auth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await auth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var api = Storage.CreatePrivateController(context);

                    identity.Data.Account.Login = "sa-sa";


                    var response =
                    (
                        await api.SaveAccountAsync(identity.Data.Account) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(response);


                    var responseGet =
                    (
                        await api.GetAccountAsync(identity.Data.Account.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(responseGet);

                    Compare(responseGet.Data, identity.Data.Account, 1);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidUpdate))]
        internal async void UpdateAcountAsync_InValid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {

                    var auth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await auth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var api = Storage.CreatePrivateController(context);

                    identity.Data.Account.Login = "sa-sa";

                    Storage.RunSql("INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login', 'password', 'salt', 'role'); ");


                    var response =
                    (
                        await api.SaveAccountAsync(account) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckFail(response);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }



        [Theory]
        [ClassData(typeof(InsertWithPagingAndResults))]
        internal async void GetAccountsAsyncPaging_Valid(string sql, Paging paging, Account[] expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    //NOTE: creating a first valid account
                    var auth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await auth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    Storage.RunSql(sql);

                    var api = Storage.CreatePrivateController(context);

                    var response =
                    (
                        await api.GetAccountsAsync(paging) as JsonResult
                    ).Value as ExecutionResult<Account[]>;
                    GenericChecks.CheckSucceed(response);

                    foreach (var item in expected)
                    {
                        var actual = response.Data.FirstOrDefault(x => x.Id == item.Id);

                        Compare(actual, item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Fact]
        internal async void UpdatePasswordAsync_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var auth = Storage.CreateGatewayController(context);
                    var identity =
                    (
                        await auth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);



                    var api = Storage.CreatePrivateController(context);

                    identity.Data.Account.Password = "sa-sa";

                    var updated =
                    (
                        await api.SaveAccountAsync(identity.Data.Account) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(updated);


                    var responseGet =
                    (
                        await api.GetAccountAsync(identity.Data.Account.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Account>;
                    GenericChecks.CheckSucceed(responseGet);

                    Compare(responseGet.Data, identity.Data.Account, 1);

                    var newIdentity =
                    (
                        await auth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = identity.Data.Account.Password }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    Compare(newIdentity.Data.Account, identity.Data.Account, 1);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        private void Compare(Account actual, Account expected, int? version = null)
        {
            Assert.NotNull(actual.Id);
            Assert.Equal(expected.Login, actual.Login);
            Assert.Null(actual.Password); // API must not return password
            Assert.Equal(expected.Role, actual.Role);

            if (version == null)
            {
                Assert.Equal(expected.Version, actual.Version);
            }
            else
            {
                Assert.Equal(version.Value, actual.Version);
            }
        }
    }
}
