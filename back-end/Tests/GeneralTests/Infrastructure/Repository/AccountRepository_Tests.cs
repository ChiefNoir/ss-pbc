using Abstractions.ISecurity;
using Abstractions.Model;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeneralTests.Infrastructure.Repository
{
    public class AccountRepository_Tests
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
                        "INSERT INTO account (id, login, password, salt, role) VALUES (1, 'login', 'password', 'salt', 'role'); "
                    }
                };
                yield return new object[]
                {
                    new[]
                    {
                        "INSERT INTO account (id, login, password, salt, role) VALUES (1, 'login', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password', 'salt', 'role'); ",
                        "INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password', 'salt', 'role'); "
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
                    "INSERT INTO account (id, login, password, salt, role) VALUES (1, 'login', 'password', 'salt', 'role'); ",
                    new Account
                    {
                        Id = 1, Login = "login", Password = null, Role = "role", Version = 0
                    }
                };
                yield return new object[]
                {
                    "INSERT INTO account (id, login, password, salt, role) VALUES (1, 'login', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (2, 'login2', 'password', 'salt', 'role'); "
                    + " INSERT INTO account (id, login, password, salt, role) VALUES (3, 'login3', 'password', 'salt', 'role'); "
                    ,
                    new Account
                    {
                        Id = 3, Login = "login3", Password = null, Role = "role", Version = 0
                    }
                };
            }
        }

        class GenerateAccount : IEnumerable<object[]>
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
            }
        }

        class GenerateInvalidCreateAccount : IEnumerable<object[]>
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
                        Password = "Password",
                        Role = "admin"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "sa",
                        Password = null,
                        Role = "admin"
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "sa",
                        Password = "password",
                        Role = null
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = null,
                        Login = "login2",
                        Password = "password",
                        Role = "role"
                    }
                };
            }
        }

        class GenerateInvalidDeleteAccount : IEnumerable<object[]>
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
                        Password = "password",
                        Role = "role",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 12,
                        Login = "login",
                        Password = "password",
                        Role = "role",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "login",
                        Password = "password",
                        Role = "role",
                        Version = 10
                    }
                };
            }
        }

        class GenerateInvalidUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "login",
                        Password = "password",
                        Role = "role",
                        Version = 10
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = null,
                        Password = "password",
                        Role = "role",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "login",
                        Password = "password",
                        Role = null,
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 1,
                        Login = "login2",
                        Password = "password",
                        Role = "role",
                        Version = 0
                    }
                };
                yield return new object[]
                {
                    new Account
                    {
                        Id = 12,
                        Login = "login",
                        Password = "password",
                        Role = "role",
                        Version = 0
                    }
                };
            }
        }



        [Theory]
        [ClassData(typeof(Inserts))]
        internal async void CountAsync_Valid(string[] sql)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    foreach (var item in sql)
                    {
                        Storage.RunSql(item);
                    }

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);
                    var result = await rep.CountAsync();

                    Assert.Equal(sql.Count(), result);
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
        internal async void GetAsync_LoginPassword_Empty_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash("sa", null))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "hexhash-sa", HexSalt = "hexsalt-sa" });

                    hashManager.Setup(x => x.Hash("sa", "hexsalt-sa"))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "hexhash-sa", HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);
                    var result = await rep.GetAsync("sa", "sa");

                    Assert.Equal("sa", result.Login);
                    Assert.Null(result.Password);
                    Assert.Equal(1, result.Id);
                    Assert.Equal("admin", result.Role);

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
        internal async void GetAsync_Id_Empty_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash("sa", null))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "hexhash-sa", HexSalt = "hexsalt-sa" });

                    hashManager.Setup(x => x.Hash("sa", "hexsalt-sa"))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "hexhash-sa", HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);
                    var result = await rep.GetAsync(0);

                    Assert.Null(result);


                    var resultCount = await rep.CountAsync();
                    Assert.Equal(0, resultCount);
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
        internal async void GetAsync_Id_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    var result = await rep.GetAsync(account.Id.Value);
                    
                    Compare(account, result);
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
        internal async void GetAsync_LoginPassword_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash(account.Login, null))
                               .Returns(new Abstractions.Model.HashResult { HexHash = account.Password, HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);
                    var result = await rep.GetAsync(account.Id.Value);

                    Compare(account, result);
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
        [ClassData(typeof(GenerateAccount))]
        internal async void SaveAsync_Valid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash(account.Password, null))
                               .Returns(new HashResult { HexHash = account.Password, HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    var result = await rep.SaveAsync(account);

                    Compare(account, result);
                }
                catch(Exception)
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
        internal async void Update_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    account.Login = "new-login";
                    
                    var result = await rep.SaveAsync(account);

                    account.Version = 1;
                    Compare(account, result);
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
        internal async void UpdatePassword_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash("very-new-password", null))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "very-new-password", HexSalt = "hexsalt-sa" });

                    hashManager.Setup(x => x.Hash("very-new-password", "hexsalt-sa"))
                               .Returns(new Abstractions.Model.HashResult { HexHash = "very-new-password", HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    account.Password = "very-new-password";
                    var result = await rep.SaveAsync(account);

                    account.Version = 1;
                    Compare(account, result);

                    var login = await rep.GetAsync(account.Login, "very-new-password");
                    Compare(account, login);
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
        internal async void Delete_Valid(string sql, Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    
                    var result = await rep.DeleteAsync(account);
                    Assert.True(result);

                    var getNonexistant = await rep.GetAsync(account.Id.Value);
                    Assert.Null(getNonexistant);
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
        [ClassData(typeof(GenerateInvalidCreateAccount))]
        internal async void CreateAsync_InValid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role)"
                        +" VALUES (1, 'login', 'password', 'salt', 'role'); "
                    );
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role)"
                        + " VALUES (2, 'login2', 'password', 'salt', 'role'); "
                    );

                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash(account.Password, null))
                               .Returns(new HashResult { HexHash = account.Password, HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.SaveAsync(account));

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
        [ClassData(typeof(GenerateInvalidDeleteAccount))]
        internal async void DeleteAsync_InValid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role)"
                        + " VALUES (1, 'login', 'password', 'salt', 'role'); "
                    );

                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash(account.Password, null))
                               .Returns(new HashResult { HexHash = account.Password, HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.DeleteAsync(account));

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
        [ClassData(typeof(GenerateInvalidUpdate))]
        internal async void UpdateAsync_InValid(Account account)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (1, 'login', 'password', 'salt', 'role', 0); "
                    );
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (2, 'login2', 'password', 'salt', 'role', 0); "
                    );

                    var hashManager = new Mock<IHashManager>();
                    hashManager.Setup(x => x.Hash(account.Password, null))
                               .Returns(new HashResult { HexHash = account.Password, HexSalt = "hexsalt-sa" });

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.SaveAsync(account));

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
        [InlineData(null, "password")]
        [InlineData("login", null)]
        [InlineData("login", "ps")]
        [InlineData("login2", "ps")]
        internal async void GetAsync_LoginPassword_InValid(string login, string password)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (1, 'login', 'password', 'salt', 'role', 0); "
                    );

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.GetAsync(login, password));

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
        internal async void SearchAsync_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql
                    (
                        "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (0, 'login-0', 'password', 'salt', 'role-0', 0); "
                        + "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (1, 'login-1', 'password', 'salt', 'role-1', 1); "
                        + "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (2, 'login-2', 'password', 'salt', 'role-2', 2); "
                        + "INSERT INTO account (id, login, password, salt, role, version)"
                        + " VALUES (3, 'login-3', 'password', 'salt', 'role-3', 3); "
                    );

                    var hashManager = new Mock<IHashManager>();

                    var rep = new AccountRepository(context, Storage.InitConfiguration(), hashManager.Object);

                    {
                        var pagingResult = await rep.SearchAsync(0, 1);
                        Assert.Single(pagingResult);
                        Assert.Equal(0, pagingResult[0].Id);
                    }

                    {
                        var pagingResult = await rep.SearchAsync(1, 2);
                        Assert.Equal(2, pagingResult.Length);
                    }

                    
                    Assert.Single(await rep.SearchAsync(2, 1));

                    Assert.Empty(await rep.SearchAsync(5, 6));

                    var result = await rep.SearchAsync(0, 20);
                    Assert.Equal(4, result.Length);

                    for (int i = 0; i < result.Length; i++)
                    {
                        Assert.Equal(i, result[i].Id);
                        Assert.Equal("login-" + i, result[i].Login);
                        Assert.Equal("role-" + i, result[i].Role);
                        Assert.Equal(i, result[i].Version);
                        Assert.Null(result[i].Password);
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



        private static void Compare(Account expected, Account actual)
        {
            Assert.Equal(expected.Login, actual.Login);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Null(actual.Password);
            Assert.Equal(expected.Version, actual.Version);

        }

    }
}
