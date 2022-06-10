using Abstractions.Model;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicCategoryController_Tests
    {
        class DefaultCategories : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new object[]
                    {
                        new Category
                        {
                            Id = 1,
                            Code = "all",
                            DisplayName = "Everything",
                            IsEverything = true,
                            TotalProjects = 1,
                            Version = 0,
                        },
                        new Category
                        {
                            Id = 2,
                            Code = "vg",
                            DisplayName = "Games",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Category
                        {
                            Id = 3,
                            Code = "ma",
                            DisplayName = "Comics",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Category
                        {
                            Id = 4,
                            Code = "lit",
                            DisplayName = "Stories",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Category
                        {
                            Id = 5,
                            Code = "bg",
                            DisplayName = "Tabletop",
                            IsEverything = false,
                            Version = 0,
                        },
                        new Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            TotalProjects = 1,
                            IsEverything = false,
                            Version = 0,
                        },
                    }
                };

            }
        }

        class DefaultEverythinCategory : IEnumerable<object[]>
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
                        Version = 0,
                    }
                };

            }
        }

        [Theory]
        [ClassData(typeof(DefaultCategories))]
        internal async Task GetCategoriesAsync(Category[] expectedCategories)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);
                    var response =
                    (
                        await api.GetCategoriesAsync() as JsonResult
                    ).Value as ExecutionResult<Category[]>;

                    GenericChecks.CheckSucceed(response);

                    foreach (var expected in expectedCategories)
                    {
                        var actual = response.Data.FirstOrDefault(x => x.Id == expected.Id);
                        Compare(expected, actual);
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
        [ClassData(typeof(DefaultCategories))]
        internal async Task GetCategory_ValidAsync(Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);

                    foreach (var expected in expectedCategory)
                    {
                        var response = 
                        (
                            await api.GetCategoryAsync(expected.Id.Value) as JsonResult
                        ).Value as ExecutionResult<Category>;
                        
                        GenericChecks.CheckSucceed(response);
                        Compare(expected, response.Data);
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
        [InlineData(10)]
        [InlineData(-10)]
        internal async Task GetCategory_InvalidAsync(int id)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);

                    var response =
                    (
                        await api.GetCategoryAsync(id) as JsonResult
                    ).Value as ExecutionResult<Category>;

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

        private void Compare(Category expected, Category actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            Assert.Equal(expected.TotalProjects, actual.TotalProjects);
            Assert.Equal(expected.Version, actual.Version);
        }
    }
}
