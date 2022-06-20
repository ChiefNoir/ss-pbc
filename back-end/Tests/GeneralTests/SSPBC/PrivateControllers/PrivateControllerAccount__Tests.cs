using Abstractions.Models;
using Abstractions.Security;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PrivateControllers
{
    [Collection("database_sensitive")]
    public sealed class PrivateControllerAccount__Tests
    {
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
                        (JsonResult)await gateway.LoginAsync(new Credentials { Login = account.Login, Password = account.Password})
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

                    var api = Initializer.CreatePrivateController(context);
                    var resultSave =
                    (
                        (JsonResult)await api.SaveAccountAsync(account)
                    ).Value as ExecutionResult<Account>;

                    Validator.CheckFail(resultSave!);

                    var gateway = Initializer.CreateGatewayController(context);
                    var resultLogin =
                    (
                        (JsonResult)await gateway.LoginAsync(new Credentials { Login = account.Login, Password = account.Password })
                    ).Value as ExecutionResult<Identity>;
                    Validator.CheckFail(resultLogin!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
