using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Private;
using API.Controllers.Public;
using API.Model;
using API.Queries;
using GeneralTests.SharedUtils;
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
        class ValidUpdate : IEnumerable<object[]>
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

        class ValidCreate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Cute things",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Cute things",
                        IsEverything = false,
                        Version = 0,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "cute_long_name",
                        DisplayName = "Very Cute things",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = null,
                        Code = "cute_long_name",
                        DisplayName = "Very Cute things",
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        class InValidCreate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Cute things",
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "vg",
                        DisplayName = "Cute things",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = null,
                        DisplayName = "Cute things",
                        IsEverything = false,
                        Version = 0,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = null,
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        class InValidUpdate : IEnumerable<object[]>
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
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 6,
                        Code = "vg",
                        DisplayName = "Software",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 6,
                        Code = "s",
                        DisplayName = "Software",
                        IsEverything = true,
                        Version = 0,
                    }
                };
            }
        }

        class ValidDelete : IEnumerable<object[]>
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

        class InValidDelete : IEnumerable<object[]>
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
                        Version = 0,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 10,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
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
                        Id = 10,
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
                        Id = 6,
                        Code = "s",
                        DisplayName = "Software",
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidUpdate))]
        internal async void Save_Valid(Category update, Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InValidUpdate))]
        internal async void Save_InValid(Category update)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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
                    context.FlushData();
                }
            }
        }


        [Theory]
        [ClassData(typeof(ValidCreate))]
        internal async void Create_Valid(Category create, Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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
                        await api.Save(identity.Data.Token, create) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.NotNull(response.Data);
                    Assert.Null(response.Error);
                    Assert.True(response.IsSucceed);
                    Compare(expected, response.Data, true);


                    var getResponse =
                    (
                        await apiPublic.GetCategory(response.Data.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;

                    Assert.NotNull(getResponse.Data);
                    Assert.Null(getResponse.Error);
                    Assert.True(getResponse.IsSucceed);
                    Compare(expected, getResponse.Data, true);

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
        [ClassData(typeof(InValidCreate))]
        internal async void Create_InValid(Category create)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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
                        await api.Save(identity.Data.Token, create) as JsonResult
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
                    context.FlushData();
                }
            }
        }



        [Theory]
        [ClassData(typeof(ValidDelete))]
        internal async void Delete_Valid(Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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
                    Assert.NotNull(getResponse.Error);
                    Assert.False(getResponse.IsSucceed);
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
        [ClassData(typeof(InValidDelete))]
        internal async void Delete_InValid(Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var confing = Storage.CreateConfiguration();
                    var categoryRep = new CategoryRepository(context);
                    var hashManager = new HashManager(confing);
                    var accountRep = new AccountRepository(context, confing, hashManager);
                    var tokenManager = new TokenManager(confing);
                    var sup = new Supervisor(tokenManager);

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

                    Assert.False(response.Data);
                    Assert.NotNull(response.Error);
                    Assert.False(response.IsSucceed);
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


        private void Compare(Category expected, Category actual, bool ignoreId = false)
        {
            if (!ignoreId)
            {
                Assert.Equal(expected.Id, actual.Id);
            }
            else
            {
                Assert.NotNull(actual.Id);
            }
            
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            Assert.Equal(expected.TotalProjects, actual.TotalProjects);
            Assert.Equal(expected.Version, actual.Version);
        }
    }
}
