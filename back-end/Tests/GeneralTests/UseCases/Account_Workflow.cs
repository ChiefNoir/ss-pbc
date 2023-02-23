using Abstractions.Models;
using Abstractions.Security;
using Microsoft.Extensions.Configuration;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePrivateController(context, cache);
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var resultLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value!.Data;

                    // Step 1: Create new account
                    var resultSaveAccount =
                    (
                        await api.SaveAccountAsync(account, resultLogin!.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckSucceed(resultSaveAccount);
                    Validator.Compare(account, resultSaveAccount.Data);
                    // *****************************

                    // Step 2: Login with new account
                    var resultLogin2 =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(resultLogin2);
                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        resultLogin2.Data!.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(resultLogin2.Data.Token);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var gateway = Initializer.CreateGatewayController(context);

                    // Step 1: Login (initialize default account)
                    var response =
                    (
                        await gateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(response);
                    Validator.Compare(Default.Account, response.Data!);
                    // *****************************

                    // Step 2: Try to add invalid account
                    var api = Initializer.CreatePrivateController(context, cache);
                    var resultSave =
                    (
                        await api.SaveAccountAsync(account, response.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(resultSave);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                    await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var gateway = Initializer.CreateGatewayController(context);

                    // Step 1: Login (initialize default account)
                    var response =
                    (
                        await gateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(response);
                    Validator.Compare(Default.Account, response.Data!);
                    // *****************************

                    // Step 2: Create filler account for tests
                    var api = Initializer.CreatePrivateController(context, cache);
                    var fillerSave =
                    (
                        await api.SaveAccountAsync
                        (
                            new Account { Login = "qq", Password = "qq", Role = RoleNames.Demo },
                            response.Data!.Token,
                            Default.Credentials.Fingerprint
                        )
                    ).Value;
                    Validator.CheckSucceed(fillerSave);
                    // *****************************

                    // Step 3: Try to update default account with invalid data
                    account.Id = response.Data!.AccountId;
                    var resultSave =
                    (
                        await api.SaveAccountAsync(account, response.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(resultSave);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Login (initialize default account)
                    var gateway = Initializer.CreateGatewayController(context);
                    var responseLogin =
                    (
                        await gateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Request account by id
                    var api = Initializer.CreatePrivateController(context, cache);
                    var resultGetAccount =
                    (
                        await api.GetAccountAsync(responseLogin.Data!.AccountId!, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckSucceed(resultGetAccount);
                    Validator.Compare(Default.Account, resultGetAccount.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Login (initialize default account)
                    var gateway = Initializer.CreateGatewayController(context);
                    var responseLogin =
                    (
                        await gateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Request account by id
                    var api = Initializer.CreatePrivateController(context, cache);
                    var resultGetAccount =
                    (
                        await api.GetAccountAsync(Guid.NewGuid(), responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckFail(resultGetAccount);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 2: Try to delete account and fail
                    var acc = new Account
                    {
                        Id = null
                    };
                    var accId = responseLogin.Data!.AccountId;
                    acc.Id = null;

                    var resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************

                    // Step 2: Try to delete account and fail
                    acc = new Account
                    {
                        Id = Guid.NewGuid()
                    };

                    resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************

                    // Step 2: Try to delete account and fail (version)
                    acc = new Account
                    {
                        Id = accId,
                        Version = 90
                    };
                    resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(acc, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(resultDel);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data!);
                    // *****************************

                    // Step 2: Try to delete account
                    var acc = new Account
                    {
                        Id = null,
                        Login = "lg",
                        Password = "ps",
                        Role = RoleNames.Admin
                    };
                    var resultAdd =
                    (
                        await apiPrivate.SaveAccountAsync(acc, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    var resultDel =
                    (
                        await apiPrivate.DeleteAccountAsync(resultAdd!.Data, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckSucceed(resultDel);
                    var resultNextGet =
                    (
                        await apiPrivate.GetAccountsAsync(responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckSucceed(resultNextGet);
                    Validator.Compare(new[] { Default.Account }, resultNextGet.Data);
                    // *****************************

                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(new Credentials(Default.Account.Login, Default.Account.Password, Default.Credentials.Fingerprint))
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data!);

                    var account = new Account
                    {
                        Id = responseLogin.Data!.AccountId,
                        Login = responseLogin.Data!.Login,
                        Role = responseLogin.Data.Role,
                        Password = "red-green-blue"
                    };
                    // *****************************

                    // Step 2: Change password
                    var resultSave =
                    (
                        await apiPrivate.SaveAccountAsync(account, responseLogin.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckSucceed(resultSave);
                    account.Version++;
                    Validator.Compare(account, resultSave.Data);
                    // *****************************

                    // Step 3: Login with a new password (win)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(new Credentials(account.Login, account.Password, Default.Credentials.Fingerprint))
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 4: Login with an old password (fail)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(new Credentials(account.Login, Default.Account.Password, Default.Credentials.Fingerprint))
                    ).Value;
                    Validator.CheckFail(responseLogin);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
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

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Login (initialize default account)
                    var responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data!);

                    var account = new Account
                    {
                        Id = responseLogin.Data!.AccountId,
                        Login = "red-green-blue",
                        Role = responseLogin.Data.Role,
                        Version = 0,
                    };
                    // *****************************

                    // Step 2: Change login
                    var resultSave =
                    (
                        await apiPrivate.SaveAccountAsync(account, responseLogin.Data.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckSucceed(resultSave);
                    account.Version++;
                    Validator.Compare(account, resultSave.Data);
                    // *****************************

                    // Step 3: Step 3: Login with a new login (win)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync
                        (
                            new Credentials(account.Login, Default.Credentials.Password, Default.Credentials.Fingerprint)
                        )
                    ).Value;
                    Validator.CheckSucceed(responseLogin);
                    // *****************************

                    // Step 4: Login with an old login (fail)
                    responseLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;
                    Validator.CheckFail(responseLogin);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }
    }
}
