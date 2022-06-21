using Abstractions.Models;
using Abstractions.Security;
using Microsoft.Extensions.Configuration;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "Work-flow")]
    [Collection("database_sensitive")]
    public sealed class Account_Workflow
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
                                Role = RoleNames.Admin,
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
                                Role = RoleNames.Demo,
                                Version = 0
                            }
                };
                yield return new object[]
                {
                            new Account
                            {
                                Id = null,
                                Login = "another-one",
                                Password = "long-password",
                                Role = RoleNames.Admin,
                                Version = 0
                            }
                };
                yield return new object[]
                {
                            new Account
                            {
                                Id = null,
                                Login = "another-one-90",
                                Password = "90-long-password",
                                Role = RoleNames.Demo,
                                Version = 0
                            }
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidAdd))]
        internal async Task AddAccount_Positive(Account account)
        {
            // Story ***********************
            // Step 1: Create new account
            // Step 2: Login with new account
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePrivateController(context);

                    // Step 1: Create new account
                    var resultSaveAccount =
                    (
                        await api.SaveAccountAsync(account)
                    ).Value;

                    Validator.CheckSucceed(resultSaveAccount);
                    Validator.Compare(account, resultSaveAccount.Data);
                    // *****************************

                    // Step 2: Login with new account
                    var gateway = Initializer.CreateGatewayController(context);
                    var resultLogin =
                    (
                        await gateway.LoginAsync(account.Login, account.Password)
                    ).Value;
                    Validator.CheckSucceed(resultLogin);
                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        resultLogin.Data.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(resultLogin.Data.Token);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
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
                        Login = "admin",
                        Password = null, // (here)
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
                        Role = "something" // (here)
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "sa", // (here)
                        Password = "sa",
                        Role = RoleNames.Admin,
                    }
                };

                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = string.Empty, // (here)
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
                        Password = string.Empty, // (here)
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
                        Role = string.Empty // (here)
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = Guid.NewGuid(),  // (here)
                        Login = "admin",
                        Password = "admin",
                        Role = RoleNames.Admin
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidAdd))]
        internal async Task AddAccount_Negative(Account account)
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Try to add invalid account
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var gateway = Initializer.CreateGatewayController(context);

                    // Step 1: Login (initialize default account)
                    var response =
                    (
                        await gateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(response);
                    Validator.Compare(Default.Account, response.Data.Account);
                    // *****************************

                    // Step 2: Try to add invalid account
                    var api = Initializer.CreatePrivateController(context);
                    var resultSave =
                    (
                        await api.SaveAccountAsync(account)
                    ).Value;
                    Validator.CheckFail(resultSave);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        private class InvalidUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account
                    {
                        Login = string.Empty, // (here)
                        Password = Default.Account.Password,
                        Role = Default.Account.Role
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Login = Default.Account.Login,
                        Password = Default.Account.Password,
                        Role = string.Empty, // (here)
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = Default.Account.Id,
                        Login = Default.Account.Login,
                        Password = Default.Account.Password,
                        Role = "nothing" // (here)
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Login = Default.Account.Login,
                        Password = "Default.Account.Password",
                        Role = Default.Account.Role,
                        Version = 10, // (here)
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Login = "qq",// (here)
                        Password = "Default.Account.Password",
                        Role = Default.Account.Role,
                        Version = 0,
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidUpdate))]
        internal async Task UpdateAccount_Negative(Account account)
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Create filler account for tests
            // Step 3: Try to update default account with invalid data
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var gateway = Initializer.CreateGatewayController(context);

                    // Step 1: Login (initialize default account)
                    var response =
                    (
                        await gateway.LoginAsync(Default.Account.Login,Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(response);
                    Validator.Compare(Default.Account, response.Data.Account);
                    // *****************************

                    // Step 2: Create filler account for tests
                    var api = Initializer.CreatePrivateController(context);
                    var fillerSave =
                    (
                        await api.SaveAccountAsync(new Account { Login = "qq", Password = "qq", Role = RoleNames.Demo })
                    ).Value;
                    Validator.CheckSucceed(fillerSave);
                    // *****************************

                    // Step 3: Try to update default account with invalid data
                    account.Id = response.Data.Account.Id;
                    var resultSave =
                    (
                        await api.SaveAccountAsync(account)
                    ).Value;
                    Validator.CheckFail(resultSave);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task RequestAccountAsync_Positive()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Request account by id
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Login (initialize default account)
                    var gateway = Initializer.CreateGatewayController(context);
                    var responseLogin =
                    (
                        await gateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Request account by id
                    var api = Initializer.CreatePrivateController(context);
                    var resultGetAccount =
                    (
                        await api.GetAccountAsync(responseLogin.Data.Account.Id.Value)
                    ).Value;

                    Validator.CheckSucceed(resultGetAccount);
                    Validator.Compare(Default.Account, resultGetAccount.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task RequestAccountAsync_Negative()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Request account by not a valid id
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Login (initialize default account)
                    var gateway = Initializer.CreateGatewayController(context);
                    var responseLogin =
                    (
                        await gateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Request account by id
                    var api = Initializer.CreatePrivateController(context);
                    var resultGetAccount =
                    (
                        await api.GetAccountAsync(Guid.NewGuid())
                    ).Value;

                    Validator.CheckFail(resultGetAccount);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task DeleteAccount_Negative()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Try to delete account and fail (id = null)
            // Step 2: Try to delete account and fail (id wrong)
            // Step 2: Try to delete account and fail (version)
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Try to delete account and fail
                    var acc = responseLogin.Data.Account;
                    var accId = responseLogin.Data.Account.Id;
                    acc.Id = null;

                    var resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************

                    // Step 2: Try to delete account and fail
                    acc = responseLogin.Data.Account;
                    acc.Id = Guid.NewGuid();

                    resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************

                    // Step 2: Try to delete account and fail (version)
                    acc = responseLogin.Data.Account;
                    acc.Id = accId;
                    acc.Version++;
                    resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task DeleteAccount_Positive()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Try to delete account
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data.Account);
                    // *****************************

                    // Step 2: Try to delete account
                    var acc = responseLogin.Data.Account;
                    var resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc)
                    ).Value;
                    Validator.CheckSucceed(resultDel);

                    var resultNextGet =
                    (
                        await apiPrivate.GetAccountsAsync()
                    ).Value;

                    Validator.CheckSucceed(resultNextGet);
                    Validator.Compare(Enumerable.Empty<Account>(), resultNextGet.Data);
                    // *****************************

                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task ChangePassword_Positive()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Change password
            // Step 3: Login with a new password (win)
            // Step 4: Login with an old password (fail)
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data.Account);

                    var account = responseLogin.Data.Account;
                    // *****************************

                    // Step 2: Change password
                    account.Password = "red-green-blue";
                    var resultSave =
                    (
                        await apiPrivate.SaveAccountAsync(account)
                    ).Value;
                    Validator.CheckSucceed(resultSave);
                    account.Version++;
                    Validator.Compare(account, resultSave.Data);
                    // *****************************

                    // Step 3: Login with a new password (win)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(account.Login, account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 4: Login with an old password (fail)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckFail(responseLogin);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task ChangeLogin_Positive()
        {
            // Story ***********************
            // Step 1: Login (initialize default account)
            // Step 2: Change login
            // Step 3: Login with a new login (win)
            // Step 4: Login with an old login (fail)
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data.Account);

                    var account = responseLogin.Data.Account;
                    // *****************************

                    // Step 2: Change login
                    account.Login = "red-green-blue";
                    var resultSave =
                    (
                        await apiPrivate.SaveAccountAsync(account)
                    ).Value;
                    Validator.CheckSucceed(resultSave);
                    account.Version++;
                    Validator.Compare(account, resultSave.Data);
                    // *****************************

                    // Step 3: Step 3: Login with a new login (win)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 4: Login with an old login (fail)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;
                    Validator.CheckFail(responseLogin);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
