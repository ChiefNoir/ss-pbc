using Abstractions.Models;
using Abstractions.Security;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PrivateControllers
{
    [Collection("database_sensitive")]
    public sealed class PrivateControllerAccount__Tests
    {
        private readonly Account DefaultAccount = new()
        {
            Login = "sa",
            Password = "sa",
            Role = RoleNames.Admin,
            Version = 0
        };

        private class ValidAdd : IEnumerable<object[]>
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
                        Role = "admin",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "admin",
                        Password = "pswrd",
                        Role = "demo",
                        Version = 0
                    }
                };
            }
        }

        private class InvalidAdd : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
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
                        Login = "sa",
                        Password = "sa",
                        Role = "demo"
                    }
                };
            }
        }


        [Fact]
        internal void GetRoles_Valid()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);
                    var response =
                    (
                        (JsonResult)api.GetRoles()
                    ).Value as ExecutionResult<List<string>>;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(RoleNames.GetRoles(), response!.Data!);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }

        }

        [Theory]
        [ClassData(typeof(ValidAdd))]
        internal async Task AddAccountAsync_ValidAsync(Account account)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);
                    var resultSave =
                    (
                        (JsonResult)await api.SaveAccountAsync(account)
                    ).Value as ExecutionResult<Account>;

                    Validator.CheckSucceed(resultSave!);
                    Validator.Compare(account, resultSave!.Data!);


                    var gateway = Initializer.CreateGatewayController(context);
                    var resultLogin =
                    (
                        (JsonResult)await gateway.LoginAsync(new Credentials { Login = account.Login, Password = account.Password })
                    ).Value as ExecutionResult<Identity>;
                    Validator.CheckSucceed(resultLogin!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidAdd))]
        internal async Task AddAccountAsync_InvalidAsync(Account account)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var gateway = Initializer.CreateGatewayController(context);
                    var response =
                    (
                        (JsonResult)await gateway.LoginAsync(new() { Login = DefaultAccount.Login, Password = DefaultAccount.Password })
                    ).Value as ExecutionResult<Identity>;
                    Validator.CheckSucceed(response!);
                    Validator.Compare(DefaultAccount, response!.Data!.Account);

                    var api = Initializer.CreatePrivateController(context);
                    var resultSave =
                    (
                        (JsonResult)await api.SaveAccountAsync(account)
                    ).Value as ExecutionResult<Account>;
                    Validator.CheckFail(resultSave!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task GetAccountsAsync_ValidAsync()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var gateway = Initializer.CreateGatewayController(context);
                    var response =
                    (
                        (JsonResult)await gateway.LoginAsync(new() { Login = DefaultAccount.Login, Password = DefaultAccount.Password })
                    ).Value as ExecutionResult<Identity>;

                    var api = Initializer.CreatePrivateController(context);
                    var resultSave =
                    (
                        (JsonResult)await api.GetAccountsAsync()
                    ).Value as ExecutionResult<Account[]>;

                    Validator.CheckSucceed(resultSave!);
                    Validator.Compare(new[] { DefaultAccount }, resultSave!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task DeleteAccountsAsync_ValidAsync()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var gateway = Initializer.CreateGatewayController(context);
                    var response =
                    (
                        (JsonResult)await gateway.LoginAsync(new() { Login = DefaultAccount.Login, Password = DefaultAccount.Password })
                    ).Value as ExecutionResult<Identity>;

                    var api = Initializer.CreatePrivateController(context);
                    var resultGet =
                    (
                        (JsonResult)await api.GetAccountsAsync()
                    ).Value as ExecutionResult<Account[]>;

                    Validator.CheckSucceed(resultGet!);
                    Validator.Compare(new[] { DefaultAccount }, resultGet!.Data!);


                    var resultDel =
                    (
                        (JsonResult)await api.DeleteAccountAsync(resultGet!.Data![0])
                    ).Value as ExecutionResult<bool>;
                    Validator.CheckSucceed(resultDel!);

                    var resultNextGet =
                    (
                        (JsonResult)await api.GetAccountsAsync()
                    ).Value as ExecutionResult<Account[]>;

                    Validator.CheckSucceed(resultNextGet!);
                    Validator.Compare(Enumerable.Empty<Account>(), resultNextGet!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task GetAccountAsync_ValidAsync()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var gateway = Initializer.CreateGatewayController(context);
                    _ =
                    (
                        (JsonResult)await gateway.LoginAsync(new() { Login = DefaultAccount.Login, Password = DefaultAccount.Password })
                    ).Value as ExecutionResult<Identity>;

                    var api = Initializer.CreatePrivateController(context);
                    var resultGetAll =
                    (
                        (JsonResult)await api.GetAccountsAsync()
                    ).Value as ExecutionResult<Account[]>;

                    Validator.CheckSucceed(resultGetAll!);
                    Validator.Compare(new[] { DefaultAccount }, resultGetAll!.Data!);

                    var resultGet =
                    (
                        (JsonResult)await api.GetAccountAsync(resultGetAll!.Data![0].Id!.Value)
                    ).Value as ExecutionResult<Account>;

                    Validator.CheckSucceed(resultGet!);
                    Validator.Compare(DefaultAccount, resultGet!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

    }
}
