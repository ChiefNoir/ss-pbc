using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Private;
using API.Controllers.Public;
using API.Model;
using API.Queries;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateAccountController_Test
    {
        class Inserts : IEnumerable<object[]>
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
                        "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login', 'password', 'salt', 'role'); "
                    }
                };
                yield return new object[]
                {
                    new[]
                    {
                        "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login2', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (id, login, password, salt, role) VALUES (4, 'login3', 'password', 'salt', 'role'); "
                    }
                };
            }
        }

        class InsertWithResults : IEnumerable<object[]>
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

        class GenerateValidAccount : IEnumerable<object[]>
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

        class GenerateRoles : IEnumerable<object[]>
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

        class InsertWithResultsAndPaging : IEnumerable<object[]>
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


        [Theory]
        [ClassData(typeof(GenerateRoles))]
        internal async void GetRolesAsync_Test_Valid(string[] expectedRoles)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var api = new PrivateAccountController(accRep, sup);

                    var response = api.GetRoles(identity.Data.Token);
                    var responseResult = (response as JsonResult).Value as ExecutionResult<List<string>>;
                    
                    Assert.NotNull(responseResult.Data);
                    Assert.Null(responseResult.Error);
                    Assert.True(responseResult.IsSucceed);

                    Assert.Equal(expectedRoles.Length, responseResult.Data.Count);

                    foreach (var item in expectedRoles)
                    {
                        var actural = responseResult.Data.First(x => x == item);
                        Assert.Equal(item, actural);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }


        [Theory]
        [ClassData(typeof(Inserts))]
        internal async void CountAsync_Test_Valid(string[] sql)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    Storage.RunSql(sql);

                    var api = new PrivateAccountController(accRep, sup);
                    
                    var response = await api.CountAccountsAsync(identity.Data.Token);
                    var responseResult = (response as JsonResult).Value as ExecutionResult<int>;
                    
                    Assert.Null(responseResult.Error);
                    Assert.True(responseResult.IsSucceed);

                    Assert.Equal(sql.Length + 1, responseResult.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidAccount))]
        internal async void AddAccountAsync_Test_Valid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);


                    var api = new PrivateAccountController(accRep, sup);
                    var responseAdd = await api.AddAccountAsync(identity.Data.Token, account);
                    var addResult = (responseAdd as JsonResult).Value as ExecutionResult<Account>;
                    Assert.NotNull(addResult.Data);
                    Assert.Null(addResult.Error);
                    Assert.True(addResult.IsSucceed);

                    Compare(addResult.Data, account);


                    var responseGet = await api.GetAccountAsync(identity.Data.Token, addResult.Data.Id.Value);
                    var getResult = (responseGet as JsonResult).Value as ExecutionResult<Account>;
                    Assert.NotNull(getResult.Data);
                    Assert.Null(getResult.Error);
                    Assert.True(getResult.IsSucceed);

                    Compare(getResult.Data, account);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InsertWithResults))]
        internal async void DeleteAccountAsync_Test_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    Storage.RunSql(sql);

                    var api = new PrivateAccountController(accRep, sup);
                    var responseDel = await api.DeleteAccountAsync(identity.Data.Token, account);
                    var delResult = (responseDel as JsonResult).Value as ExecutionResult<bool>;
                    Assert.True(delResult.Data);
                    Assert.Null(delResult.Error);
                    Assert.True(delResult.IsSucceed);


                    var responseGet = await api.GetAccountAsync(identity.Data.Token, account.Id.Value);
                    var getResult = (responseGet as JsonResult).Value as ExecutionResult<Account>;

                    Assert.Null(getResult.Data);
                    Assert.Null(getResult.Error);
                    Assert.True(getResult.IsSucceed);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InsertWithResults))]
        internal async void GetAccountAsync_Test_Valid(string sql, Account expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    Storage.RunSql(sql);

                    var api = new PrivateAccountController(accRep, sup);
                    var responseGet = await api.GetAccountAsync(identity.Data.Token, expected.Id.Value);
                    var getResult = (responseGet as JsonResult).Value as ExecutionResult<Account>;
                    Assert.NotNull(getResult.Data);
                    Assert.Null(getResult.Error);
                    Assert.True(getResult.IsSucceed);

                    Compare(getResult.Data, expected);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InsertWithResultsAndPaging))]
        internal async void GetAccountsAsyncPaging_Valid(string sql, Paging paging, Account[] expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    Storage.RunSql(sql);

                    var api = new PrivateAccountController(accRep, sup);
                    var responseGet = await api.GetAccountsAsync(identity.Data.Token, paging);
                    var getResult = (responseGet as JsonResult).Value as ExecutionResult<Account[]>;
                    Assert.NotNull(getResult.Data);
                    Assert.Null(getResult.Error);
                    Assert.True(getResult.IsSucceed);
                    Assert.Equal(expected.Length, getResult.Data.Length);

                    foreach (var item in expected)
                    {
                        var actual = getResult.Data.FirstOrDefault(x => x.Id == item.Id);

                        Compare(actual, item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
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
                    var confing = Storage.InitConfiguration();
                    var log = new LogRepository(confing);
                    var tokenManager = new TokenManager(confing);
                    var hasManager = new HashManager();
                    var sup = new Supervisor(log, tokenManager);
                    var accRep = new AccountRepository(context, confing, hasManager);
                    var apiAuth = new AuthenticationController(confing, accRep, sup, tokenManager);


                    var loginResponse = await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" });
                    var identity = (loginResponse as JsonResult).Value as ExecutionResult<Identity>;
                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var newAccount = new Account
                    {
                        Id = 1,
                        Login = "sa-sa",
                        Role = "admin",
                        Version = 0
                    };


                    var api = new PrivateAccountController(accRep, sup);
                    var response = await api.UpdateAccountAsync(identity.Data.Token, newAccount);
                    var responseUpdate = (response as JsonResult).Value as ExecutionResult<Account>;
                    Assert.NotNull(responseUpdate.Data);
                    Assert.Null(responseUpdate.Error);
                    Assert.True(responseUpdate.IsSucceed);
                    Compare(responseUpdate.Data, newAccount, 1);

                    var responseGet = await api.GetAccountAsync(identity.Data.Token, newAccount.Id.Value);
                    var exResultresponseGet = (responseGet as JsonResult).Value as ExecutionResult<Account>;
                    Assert.NotNull(exResultresponseGet.Data);
                    Assert.Null(exResultresponseGet.Error);
                    Assert.True(exResultresponseGet.IsSucceed);




                    Compare(exResultresponseGet.Data, newAccount, 1);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }




        private void Compare(Account actual, Account expected, int? version = null)
        {
            Assert.NotNull(actual.Id);
            Assert.Equal(expected.Login, actual.Login);
            Assert.Null(actual.Password);
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
