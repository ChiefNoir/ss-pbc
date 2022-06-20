using Abstractions.Models;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PrivateControllers
{
    [Collection("database_sensitive")]
    public sealed class PrivateControllerCategory__Tests
    {
        private class ValidCreate : IEnumerable<object[]>
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
                        TotalProjects = 0,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Cute things",
                        IsEverything = false,
                        TotalProjects = 0,
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
                        TotalProjects = 0,
                        Version = 0,
                    },
                    new Category
                    {
                        Id = null,
                        Code = "cute_long_name",
                        DisplayName = "Very Cute things",
                        IsEverything = false,
                        TotalProjects = 0,
                        Version = 0,
                    },
                };
            }
        }

        private class InvalidCreate : IEnumerable<object[]>
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
                        Code = "all",
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
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = String.Empty,
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
                        DisplayName = String.Empty,
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        private class InvalidDelete : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = M197101010000_Default.categoryId,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 0,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = M197101010000_Default.categoryId,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        TotalProjects = 0,
                        Version = 10,
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
                        TotalProjects = 0,
                        Version = 0,
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidCreate))]
        internal async Task Create_ValidAsync(Category create, Category expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);
                    var response =
                    (
                        (JsonResult)await api.SaveCategoryAsync(create)
                    ).Value as ExecutionResult<Category>;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(expected, response!.Data!);

                    var apiPublic = Initializer.CreatePublicController(context);
                    var responsePublic =
                    (
                        (JsonResult)await apiPublic.GetCategoriesAsync()
                    ).Value as ExecutionResult<Category[]>;

                    var category = responsePublic!.Data!.First(x => x.Code == create.Code);
                    Validator.Compare(expected, category);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidCreate))]
        internal async Task Create_InvalidAsync(Category update)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);
                    var response =
                    (
                        (JsonResult)await api.SaveCategoryAsync(update)
                    ).Value as ExecutionResult<Category>;

                    Validator.CheckFail(response!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidDelete))]
        internal async Task Delete_InvalidAsync(Category update)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePrivateController(context);
                    var response =
                    (
                        (JsonResult)await api.DeleteCategoryAsync(update)
                    ).Value as ExecutionResult<bool>;

                    Validator.CheckFail(response!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
