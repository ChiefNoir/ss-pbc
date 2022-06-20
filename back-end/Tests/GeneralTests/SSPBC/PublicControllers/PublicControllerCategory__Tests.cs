using Abstractions.Models;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PublicControllers
{
    [Collection("database_sensitive")]
    public sealed class PublicControllerCategory__Tests
    {
        private class DefaultCategories_EmptyDatabase : IEnumerable<object[]>
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
                            Id = M197101010000_Default.categoryId,
                            Code = "all",
                            DisplayName = "Everything",
                            IsEverything = true,
                            TotalProjects = 0,
                            Version = 0,
                        },
                    }
                };
            }
        }

        private class InvalidIds_EmptyDatabase : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new object[]
                    {
                        null
                    }
                };
                yield return new object[]
                {
                    new object[]
                    {
                        Guid.Empty
                    }
                };
                yield return new object[]
                {
                    new object[]
                    {
                        Guid.NewGuid()
                    }
                };
            }
        }



        [Theory]
        [ClassData(typeof(DefaultCategories_EmptyDatabase))]
        internal async Task GetCategoriesAsync__Valid(Category[] expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePublicController(context);

                    var response =
                    (
                        (JsonResult)await api.GetCategoriesAsync()
                    ).Value as ExecutionResult<Category[]>;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(expected, response!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(DefaultCategories_EmptyDatabase))]
        internal async Task GetCategoryAsync__Valid(Category[] expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePublicController(context);

                    foreach (var item in expected)
                    {
                        var response =
                        (
                            (JsonResult)await api.GetCategoryAsync(item.Id!.Value)
                        ).Value as ExecutionResult<Category>;

                        Validator.CheckSucceed(response);
                        Validator.Compare(item, response!.Data!);
                    }
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidIds_EmptyDatabase))]
        internal async Task GetCategoryAsync__Invalid(Guid?[] ids)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePublicController(context);

                    foreach (var item in ids)
                    {
                        var response =
                        (
                            (JsonResult)await api.GetCategoryAsync(item)
                        ).Value as ExecutionResult<Category>;

                        Validator.CheckFail(response!);
                    }
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

    }
}
