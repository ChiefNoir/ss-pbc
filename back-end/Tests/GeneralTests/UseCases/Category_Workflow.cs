using Abstractions.Models;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class Category_Workflow
    {
        [Fact]
        internal async Task CheckDefault_Positive()
        {
            // Story ***********************
            // Step 1: Request categories
            // Step 2: Request category by id
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var apiPublic = Initializer.CreatePublicController(context, cache);

                    // Step 1: Request categories
                    var publiGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetCategories);
                    Validator.Compare(new[] { Default.Category }, publiGetCategories.Data);
                    // *****************************

                    // Step 2: Request category by id
                    var publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(Default.Category.Id)
                    ).Value;

                    Validator.CheckSucceed(publiGetCategory);
                    Validator.Compare(Default.Category, publiGetCategory.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task CheckDefault_Negative()
        {
            // Story ***********************
            // Step 1: Request category by invalid id
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var apiPublic = Initializer.CreatePublicController(context, cache);

                    // Step 1: Request category by invalid id
                    var publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(Guid.Empty)
                    ).Value;

                    Validator.CheckFail(publiGetCategory);
                    // *****************************

                    // Step 2: Request category by invalid id
                    publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(Guid.NewGuid())
                    ).Value;

                    Validator.CheckFail(publiGetCategory);
                    // *****************************

                    // Step 2: Request category by invalid id
                    publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(null)
                    ).Value;

                    Validator.CheckFail(publiGetCategory);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task CreateNewCategory_Positive()
        {
            // Story ***********************
            // Step 1: Create new category and save
            // Step 2: Request categories
            // Step 3: Request category by id
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Create new category and save
                    var newCategory = new Category
                    {
                        Code = "cute",
                        DisplayName = "Cute Display Name",
                        IsEverything = false,
                        Id = null
                    };

                    var apiPrivate = Initializer.CreatePrivateController(context, cache);
                    var responseSaveCategory =
                    (
                        await apiPrivate.SaveCategoryAsync(newCategory)
                    ).Value;
                    Validator.CheckSucceed(responseSaveCategory);
                    Validator.Compare(newCategory, responseSaveCategory.Data);
                    newCategory = responseSaveCategory.Data;
                    // *****************************

                    // Step 1: and save
                    newCategory.Code = "ss";

                    responseSaveCategory =
                    (
                        await apiPrivate.SaveCategoryAsync(newCategory)
                    ).Value;
                    Validator.CheckSucceed(responseSaveCategory);

                    newCategory.Version++;
                    Validator.Compare(newCategory, responseSaveCategory.Data);
                    newCategory = responseSaveCategory.Data;
                    // *****************************

                    // Step 2: Request categories
                    var apiPublic = Initializer.CreatePublicController(context, cache);
                    var publiGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetCategories);
                    Validator.Compare(new[] { Default.Category, newCategory }, publiGetCategories.Data);
                    // *****************************

                    // Step 2: Request category by id
                    var publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(newCategory.Id)
                    ).Value;

                    Validator.CheckSucceed(publiGetCategory);
                    Validator.Compare(newCategory, publiGetCategory.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task DeleteNewCategory_Positive()
        {
            // Story ***********************
            // Step 1: Create new category and save
            // Step 2: Delete new category
            // Step 3: Request category by id
            // Step 4: Request all categories
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);
                    var apiPublic = Initializer.CreatePublicController(context, cache);

                    // Step 1: Create new category and save
                    var newCategory = new Category
                    {
                        Code = "cute",
                        DisplayName = "Cute Display Name",
                        IsEverything = false,
                        Id = null
                    };

                    var responseSaveCategory =
                    (
                        await apiPrivate.SaveCategoryAsync(newCategory)
                    ).Value;
                    Validator.CheckSucceed(responseSaveCategory);
                    Validator.Compare(newCategory, responseSaveCategory.Data);
                    newCategory = responseSaveCategory.Data;
                    // *****************************

                    // Step 2: Delete category
                    var responseDeleteCategory =
                    (
                        await apiPrivate.DeleteCategoryAsync(newCategory)
                    ).Value;
                    Validator.CheckSucceed(responseDeleteCategory);
                    // *****************************

                    // Step 3: Request category by id
                    var publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(newCategory.Id)
                    ).Value;

                    Validator.CheckFail(publiGetCategory);
                    // *****************************

                    // Step 4: Request all categories
                    var publiGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetCategories);
                    Validator.Compare(new[] { Default.Category }, publiGetCategories.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
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
                        IsEverything = true, // (here)
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null,
                        Code = "all", // (here)
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
                        Code = string.Empty, // (here)
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
                        DisplayName = null,// (here)
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
                        DisplayName = string.Empty, // (here)
                        IsEverything = false,
                        Version = 0,
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true,
                        Version = 10, // (here)
                    },
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = false, // (here)
                        Version = 0,
                    },
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidCreate))]
        internal async Task SaveCategory_Negative(Category update)
        {
            // Story ***********************
            // Step 1: Create/update category and fail to save
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Create/update category and fail to save
                    var response = (await api.SaveCategoryAsync(update)).Value;

                    Validator.CheckFail(response);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
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
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true, // (here)
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true, // (here)
                        Version = 10, // (here)
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = null
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Guid.Empty
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Guid.NewGuid()
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidDelete))]
        internal async Task DeleteCategory_Negative(Category update)
        {
            // Story ***********************
            // Step 1: Fail to delete category
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Create/update category and fail to save
                    var response = (await api.DeleteCategoryAsync(update)).Value;
                    Validator.CheckFail(response);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }


        private class InvaliUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true,
                        Version = 10, // (here)
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = null, // (here)
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = string.Empty, // (here)
                        DisplayName = Default.Category.DisplayName,
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = string.Empty,  // (here)
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = Default.Category.Code,
                        DisplayName = null,  // (here)
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Guid.NewGuid(),// (here)
                        Code = Default.Category.Code,
                        DisplayName = "red",
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Category
                    {
                        Id = Default.Category.Id,
                        Code = "code", // (here)
                        DisplayName = "things",
                        IsEverything = true,
                        Version = 0,
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvaliUpdate))]
        internal async Task UpdateCategory_Negative(Category update)
        {
            // Story ***********************
            // Step 1: Fail to update category
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var api = Initializer.CreatePrivateController(context, cache);
                    var filler = (await api.SaveCategoryAsync(new Category { Code = "code", DisplayName = "name" })).Value;
                    Validator.CheckSucceed(filler);

                    // Step 1: Create/update category and fail to save
                    var response = (await api.SaveCategoryAsync(update)).Value;

                    Validator.CheckFail(response);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }

        [Fact]
        internal async Task UpdateCategory_SetEverythingTrue_Negative()
        {
            // Story ***********************
            // Step 1: Create and save new category
            // Step 2: Set new Category to IsEverything = true
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);

                    // Step 1: Fail to update category
                    var responseSaveCategoryAsync =
                    (
                        await apiPrivate.SaveCategoryAsync(new Category { Code = "code", DisplayName = "name" })
                    ).Value;
                    Validator.CheckSucceed(responseSaveCategoryAsync);
                    //*****************************

                    // Step 2: Set new Category to IsEverything = true
                    var newCat = responseSaveCategoryAsync.Data;
                    newCat.IsEverything = true;

                    var response = (await apiPrivate.SaveCategoryAsync(newCat)).Value;
                    Validator.CheckFail(response);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }


        [Fact]
        internal async Task DeleteCategoryWithProjects_Negative()
        {
            // Story ***********************
            // Step 1: Create and save new category
            // Step 2: Create new project, assign it to the category, save
            // Step 3: Fail to delete category
            // Step 4: Request category
            // Step 5: Request project
            // *****************************

            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);
                    var apiPublic = Initializer.CreatePublicController(context, cache);

                    // Step 1: Create and save new category
                    var responseSaveCategory =
                    (
                        await apiPrivate.SaveCategoryAsync(new Category { Code = "code", DisplayName = "name" })
                    ).Value;
                    Validator.CheckSucceed(responseSaveCategory);
                    var newCat = responseSaveCategory.Data;
                    //*****************************

                    // Step 2: Create new project, assign it to the category, save
                    var prj = new Project
                    {
                        Code = "prj",
                        Name = "name",
                        Description = "descr",
                        DescriptionShort = "descr short",
                        Category = newCat
                    };
                    var responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    // *****************************

                    // Step 3: Fail to delete category
                    var responseDeleteCategory =
                    (
                        await apiPrivate.DeleteCategoryAsync(newCat)
                    ).Value;
                    Validator.CheckFail(responseDeleteCategory);
                    // *****************************

                    // Step 4: Request category
                    newCat.TotalProjects++;
                    var publiGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(newCat.Id)
                    ).Value;

                    Validator.CheckSucceed(publiGetCategory);
                    Validator.Compare(newCat, publiGetCategory.Data);
                    // *****************************


                    // Step 5: Request project
                    var publiGetProject =
                    (
                        await apiPublic.GetProjectAsync("prj")
                    ).Value;
                    Validator.CheckSucceed(publiGetProject);

                    prj.Category.TotalProjects = -1; // TODO: fix it
                    Validator.Compare(prj, publiGetProject.Data);
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
