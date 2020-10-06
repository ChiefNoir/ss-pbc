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
    public class PrivateCategoryController_Test
    {
        class ValidCategoryUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "All",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "All",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 1,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = "all_and_all",
                        DisplayName = "All",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = 1,
                        Code = "all_and_all",
                        DisplayName = "All",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 1,
                    },
                };
            }
        }

        class InValidCategoryUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 10,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = false,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = null,
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 1,
                        Code = null,
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = -1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    }
                };
            }
        }

        class ValidCategoryDelete : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 3,
                        Code = "ma",
                        DisplayName = "Comics",
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }


        class BadTokens : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    null
                };
                yield return new object[]
                {
                    "bad-token"
                };
                yield return new object[]
                {
                    string.Empty
                };
                yield return new object[]
                {
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTk5Njk3MSwiZXhwIjoxNjAxOTk4NzcxLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.DCbppW8SqvL1QJS2BIO2qlplZv-UHqI2_NP_Za0KDzA"
                };
                
            }
        }

        [Theory]
        [ClassData(typeof(ValidCategoryUpdate))]
        internal async void Save_Valid(Category update, Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateCategoryController(categoryRep, sup);
                    var apiPublic = new PublicCategoryController(categoryRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var response =
                    (
                        await api.Save(identity.Data.Token, update) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.NotNull(response.Data);
                    Assert.Null(response.Error);
                    Assert.True(response.IsSucceed);
                    Compare(expected, response.Data);


                    var getResponse =
                    (
                        await apiPublic.GetCategory(update.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.NotNull(getResponse.Data);
                    Assert.Null(getResponse.Error);
                    Assert.True(getResponse.IsSucceed);
                    Compare(expected, getResponse.Data);

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
        [ClassData(typeof(InValidCategoryUpdate))]
        internal async void Save_InValid(Category update)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateCategoryController(categoryRep, sup);
                    var apiPublic = new PublicCategoryController(categoryRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var response =
                    (
                        await api.Save(identity.Data.Token, update) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.Null(response.Data);
                    Assert.NotNull(response.Error);
                    Assert.False(response.IsSucceed);
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
        [ClassData(typeof(BadTokens))]
        internal async void Save_BadToken(string badToken)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateCategoryController(categoryRep, sup);
                    var apiPublic = new PublicCategoryController(categoryRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);


                    var update = new Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "All",
                        IsEverything = true,
                        TotalProjects = 1,
                        Version = 0,
                    };

                    var response =
                    (
                        await api.Save(badToken, update) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.Null(response.Data);
                    Assert.NotNull(response.Error);
                    Assert.False(response.IsSucceed);
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
        [ClassData(typeof(ValidCategoryDelete))]
        internal async void Delete_Valid(Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.InitConfiguration();
                    var logRep = new LogRepository(confing);
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager();
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(logRep, tokenManager);

                    var api = new PrivateCategoryController(categoryRep, sup);
                    var apiPublic = new PublicCategoryController(categoryRep, sup);
                    var apiAuth = new AuthenticationController(confing, accountRep, sup, tokenManager);

                    var identity =
                    (
                        await apiAuth.LoginAsync
                        (
                            new Credentials { Login = "sa", Password = "sa" }
                        ) as JsonResult
                    ).Value as ExecutionResult<Identity>;

                    Assert.NotNull(identity.Data);
                    Assert.Null(identity.Error);
                    Assert.True(identity.IsSucceed);

                    var response =
                    (
                        await api.Delete(identity.Data.Token, category) as JsonResult
                    ).Value as ExecutionResult<bool>;

                    Assert.True(response.Data);
                    Assert.Null(response.Error);
                    Assert.True(response.IsSucceed);
                    

                    var getResponse =
                    (
                        await apiPublic.GetCategory(category.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.Null(getResponse.Data);
                    Assert.Null(getResponse.Error);
                    Assert.True(getResponse.IsSucceed);
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


        private void Compare(Category expected, Category actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            Assert.Equal(expected.TotalProjects, actual.TotalProjects);
            Assert.Equal(expected.Version, actual.Version);
        }
    }
}
