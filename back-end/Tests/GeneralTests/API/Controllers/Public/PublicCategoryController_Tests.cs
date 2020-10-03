using Abstractions.Supervision;
using API.Controllers.Public;
using GeneralTests.Utils;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicCategoryController_Tests
    {
        class CreateValidDefaults : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new object[]
                    {
                        new Abstractions.Model.Category
                        {
                            Id = 1,
                            Code = "all",
                            DisplayName = "Everything",
                            IsEverything = true,
                            Version = 0,
                        },
                        new Abstractions.Model.Category
                        {
                            Id = 2,
                            Code = "vg",
                            DisplayName = "Games",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Abstractions.Model.Category
                        {
                            Id = 3,
                            Code = "ma",
                            DisplayName = "Comics",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Abstractions.Model.Category
                        {
                            Id = 4,
                            Code = "lit",
                            DisplayName = "Stories",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Abstractions.Model.Category
                        {
                            Id = 5,
                            Code = "bg",
                            DisplayName = "Tabletop",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Abstractions.Model.Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            IsEverything = false,
                            Version = 0,
                        },
                    }
                };

            }
        }

        class CreateEverythinCategory : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                        {
                            Id = 1,
                            Code = "all",
                            DisplayName = "Everything",
                            IsEverything = true,
                            Version = 0,
                        }
                };

            }
        }


        [Theory]
        [ClassData(typeof(CreateValidDefaults))]
        internal async void GetCategories_Valid(Abstractions.Model.Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);
                    var log = new LogRepository(Storage.InitConfiguration());
                    var tokenManager = new TokenManager(Storage.InitConfiguration());
                    var sup = new Supervisor(log, tokenManager);

                    var api = new PublicCategoryController(rep, sup);

                    var response = await api.GetCategories();

                    var result = (response as JsonResult).Value as ExecutionResult<Abstractions.Model.Category[]>;

                    foreach (var expected in expectedCategory)
                    {
                        var actual = result.Data.FirstOrDefault(x => x.Id == expected.Id);

                        Assert.True(result.IsSucceed);
                        Assert.Null(result.Error);
                        Compare(expected, actual);
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
        [ClassData(typeof(CreateValidDefaults))]
        internal async void GetCategory_Valid(Abstractions.Model.Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);
                    var log = new LogRepository(Storage.InitConfiguration());
                    var tokenManager = new TokenManager(Storage.InitConfiguration());
                    var sup = new Supervisor(log, tokenManager);

                    var api = new PublicCategoryController(rep, sup);

                    foreach (var expected in expectedCategory)
                    {
                        var response = await api.GetCategory(expected.Id.Value);
                        var result = (response as JsonResult).Value as ExecutionResult<Abstractions.Model.Category>;

                        Assert.True(result.IsSucceed);
                        Assert.Null(result.Error);
                        Compare(expected, result.Data);
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
        [ClassData(typeof(CreateEverythinCategory))]
        internal async void GetEverythingCategory_Valid(Abstractions.Model.Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);
                    var log = new LogRepository(Storage.InitConfiguration());
                    var tokenManager = new TokenManager(Storage.InitConfiguration());
                    var sup = new Supervisor(log, tokenManager);

                    var api = new PublicCategoryController(rep, sup);

                    var response = await api.GetEverythingCategory();
                    var result = (response as JsonResult).Value as ExecutionResult<Abstractions.Model.Category>;

                    Assert.True(result.IsSucceed);
                    Assert.Null(result.Error);
                    Compare(expected, result.Data);

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


        private void Compare(Abstractions.Model.Category expected, Abstractions.Model.Category actual)
        {
            Assert.Equal(expected.Code, expected.Code);
            Assert.Equal(expected.DisplayName, expected.DisplayName);
            Assert.Equal(expected.IsEverything, expected.IsEverything);
            Assert.Equal(expected.TotalProjects, expected.TotalProjects);
            Assert.Equal(expected.Version, expected.Version);
        }
    }
}
