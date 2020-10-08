using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Public;
using GeneralTests.SharedUtils;
using Infrastructure;
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

        private static PublicCategoryController CreatePublicCategoryController(DataContext context)
        {
            var categoryRep = new CategoryRepository(context);
            var tokenManager = new TokenManager(Storage.CreateConfiguration());
            var sup = new Supervisor(tokenManager);

            return new PublicCategoryController(categoryRep, sup);
        }

        [Theory]
        [ClassData(typeof(DefaultCategories))]
        internal async void GetCategories(Category[] expectedCategories)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = CreatePublicCategoryController(context);
                    var response =
                    (
                        await api.GetCategories() as JsonResult
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
        internal async void GetCategory_Valid(Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = CreatePublicCategoryController(context);

                    foreach (var expected in expectedCategory)
                    {
                        var response = 
                        (
                            await api.GetCategory(expected.Id.Value) as JsonResult
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
        internal async void GetCategory_Invalid(int id)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = CreatePublicCategoryController(context);

                    var response =
                    (
                        await api.GetCategory(id) as JsonResult
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



        [Theory]
        [ClassData(typeof(DefaultEverythinCategory))]
        internal async void GetEverythingCategory(Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = CreatePublicCategoryController(context);

                    var response =
                    (
                        await api.GetEverythingCategory() as JsonResult
                    ).Value as ExecutionResult<Category>;

                    GenericChecks.CheckSucceed(response);
                    Compare(expected, response.Data);
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
