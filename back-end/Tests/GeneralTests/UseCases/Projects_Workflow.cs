using Abstractions.Models;
using SSPBC.Models;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class Projects_Workflow
    {
        [Fact]
        internal async Task CheckDefault_Positive()
        {
            // Story ***********************
            // Step 1: Request projects preview from an empty database
            // Step 2: Request categories an empty database (Total projects count in system category = 0)
            // Step 3: Request projects preview from an empty database (with category code)
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Request projects preview from an empty database
                    for (int i = 0; i < 100; i++)
                    {
                        var publicGetProjectsPreview =
                        (
                            await apiPublic.GetProjectsPreviewAsync
                                    (
                                        new Paging { Start = i, Length = 1 },
                                        new ProjectSearch { CategoryCode = null }
                                    )
                        ).Value;

                        Validator.CheckSucceed(publicGetProjectsPreview);
                        Validator.Compare(Enumerable.Empty<ProjectPreview>(), publicGetProjectsPreview.Data);
                    }
                    // *****************************

                    // Step 2: Request categories an empty database (Total projects count in system category = 0)
                    var publicGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;
                    Validator.CheckSucceed(publicGetCategories);
                    Validator.Compare(new[] { Default.Category }, publicGetCategories.Data);

                    var defaultProject = publicGetCategories.Data.First();
                    Assert.Equal(0, defaultProject.TotalProjects);
                    // *****************************

                    // Step 3: Request projects preview from an empty database (with category code)
                    for (int i = 0; i < 100; i++)
                    {
                        var publicGetProjectsPreview =
                        (
                            await apiPublic.GetProjectsPreviewAsync
                                    (
                                        new Paging { Start = i, Length = 1 },
                                        new ProjectSearch { CategoryCode = defaultProject.Code }
                                    )
                        ).Value;

                        Validator.CheckSucceed(publicGetProjectsPreview);
                        Validator.Compare(Enumerable.Empty<ProjectPreview>(), publicGetProjectsPreview.Data);
                    }
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task CreateNewProject_Positive()
        {
            // Story ***********************
            // Step 1: Request projects preview from an empty database
            // Step 2: Request categories an empty database (Total projects count in system category = 0)
            // Step 3: Create new category
            // Step 3: Create new project and assign it to the new category
            // Step 4: Check project (by code)
            // Step 4: Check project preview
            // Step 4: Check category
            // Step 4: Check category==all
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);

                    // Step 1: Request projects preview from an empty database
                    var publicGetProjectsPreview =
                        (
                            await apiPublic.GetProjectsPreviewAsync
                                    (
                                        new Paging { Start = 0, Length = 1000 },
                                        new ProjectSearch { CategoryCode = null }
                                    )
                        ).Value;

                    Validator.CheckSucceed(publicGetProjectsPreview);
                    Validator.Compare(Enumerable.Empty<ProjectPreview>(), publicGetProjectsPreview.Data);
                    // *****************************


                    // Step 2: Request categories an empty database (Total projects count in system category = 0)
                    var publicGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;
                    Validator.CheckSucceed(publicGetCategories);
                    Validator.Compare(new[] { Default.Category }, publicGetCategories.Data);

                    var defaultProject = publicGetCategories.Data.First();
                    Assert.Equal(0, defaultProject.TotalProjects);
                    // *****************************

                    await Creator.CreateNewProject(context, "code");
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task DeleteNewProject_Positive()
        {
            // Story ***********************
            // Step 1: Request projects preview from an empty database
            // Step 2: Request categories an empty database (Total projects count in system category = 0)
            // Step 3: Create new category
            // Step 3: Create new project and assign it to the new category
            // Step 4: Check project (by code)
            // Step 4: Check project preview
            // Step 4: Check category
            // Step 4: Check category==all
            // Step 5: Delete project
            // Step 5: Check project (by code)
            // Step 5: Check project preview
            // Step 5: Check category
            // Step 5: Check category==all
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    var prj = await Creator.CreateNewProject(context, "code");
                    var newCategory =
                    (
                        await apiPublic.GetCategoryAsync(prj.Category.Id)
                    ).Value.Data;

                    // Step 5: Delete project
                    var responseDeleteProject =
                    (
                        await apiPrivate.DeleteProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseDeleteProject);
                    // *****************************

                    // Step 4: Check project (by code)
                    var responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckFail(responseGetProject);
                    // *****************************

                    // Step 4: Check project preview
                    var responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Empty(responseGetProjectsPreview.Data);
                    // *****************************

                    // Step 4: Check category
                    var responseGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(newCategory.Id)
                    ).Value;
                    Validator.CheckSucceed(responseGetCategory);
                    Assert.Equal(0, responseGetCategory.Data.TotalProjects);
                    // *****************************

                    // Step 4: Check category==all
                    var responseGetCategoryDef =
                    (
                        await apiPublic.GetCategoryAsync(Default.Category.Id)
                    ).Value;
                    Validator.CheckSucceed(responseGetCategoryDef);
                    Assert.Equal(0, responseGetCategoryDef.Data.TotalProjects);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }


        [Fact]
        internal async Task EditProject_Positive()
        {
            // TODO: Too long, break it to smaller tests
            // Story ***********************
            // Step 1: Request projects preview from an empty database
            // Step 2: Request categories an empty database (Total projects count in system category = 0)
            // Step 3: Create new category
            // Step 3: Create new project and assign it to the new category
            // Step 4: Check project (by code)
            // Step 5: Check project preview
            // Step 6: Edit project 
            // Step 6: Check project (by code)
            // Step 6: Check project preview
            // Step 6: Check category
            // Step 6: Check category==all
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 1: Request projects preview from an empty database
                    var publicGetProjectsPreview =
                        (
                            await apiPublic.GetProjectsPreviewAsync
                                    (
                                        new Paging { Start = 0, Length = 1000 },
                                        new ProjectSearch { CategoryCode = null }
                                    )
                        ).Value;

                    Validator.CheckSucceed(publicGetProjectsPreview);
                    Validator.Compare(Enumerable.Empty<ProjectPreview>(), publicGetProjectsPreview.Data);
                    // *****************************

                    // Step 2: Request categories an empty database (Total projects count in system category = 0)
                    var publicGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;
                    Validator.CheckSucceed(publicGetCategories);
                    Validator.Compare(new[] { Default.Category }, publicGetCategories.Data);

                    var defaultProject = publicGetCategories.Data.First();
                    Assert.Equal(0, defaultProject.TotalProjects);
                    // *****************************

                    // Step 3: Create new category
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

                    // Step 3: Create new project and assign it to the new category
                    var prj = new Project
                    {
                        Code = "prj",
                        DisplayName = "name",
                        Description = "Description",
                        PosterDescription = "Poster-Description",
                        PosterUrl = "http://localhost/image.png",
                        ReleaseDate = new DateTime(2000, 12, 12),
                        DescriptionShort = "descr short",
                        Category = newCategory
                    };
                    var responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseSaveProject.Data);
                    // *****************************

                    // Step 4: Check project (by code)
                    var responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckSucceed(responseGetProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseGetProject.Data);
                    prj = responseGetProject.Data;
                    // *****************************

                    // Step 4: Check project preview
                    var responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Single(responseGetProjectsPreview.Data);

                    var preview = new ProjectPreview
                    {
                        Category = prj.Category,
                        Code = prj.Code,
                        Description = prj.DescriptionShort,
                        DisplayName = prj.DisplayName,
                        PosterDescription = prj.PosterDescription,
                        PosterUrl = prj.PosterUrl,
                        ReleaseDate = prj.ReleaseDate
                    };

                    Validator.Compare(preview, responseGetProjectsPreview.Data[0]);
                    // *****************************

                    // Step 6: Edit project 
                    prj.Code = "prj1";
                    prj.DisplayName = "new name";
                    prj.Description = "Description-Description";
                    prj.PosterDescription = "Poster-Description Poster-Description";
                    prj.PosterUrl = "http://localhost/image90.png";
                    prj.ReleaseDate = new DateTime(2020, 12, 12);
                    prj.DescriptionShort = "DescriptionShort DescriptionShort";

                    responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    prj.Category.TotalProjects = -1;
                    prj.Version++;
                    Validator.Compare(prj, responseSaveProject.Data);
                    // *****************************

                    // Step 4: Check project (by code)
                    responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckSucceed(responseGetProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseGetProject.Data);
                    // *****************************

                    // Step 4: Check project preview
                    responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Single(responseGetProjectsPreview.Data);

                    preview = new ProjectPreview
                    {
                        Category = prj.Category,
                        Code = prj.Code,
                        Description = prj.DescriptionShort,
                        DisplayName = prj.DisplayName,
                        PosterDescription = prj.PosterDescription,
                        PosterUrl = prj.PosterUrl,
                        ReleaseDate = prj.ReleaseDate
                    };

                    Validator.Compare(preview, responseGetProjectsPreview.Data[0]);
                    prj = responseGetProject.Data;
                    // *****************************

                    // Step 6: Edit project 
                    prj.Code = "prj1";
                    prj.DisplayName = "new name";
                    prj.Description = "Description-Description";
                    prj.PosterDescription = "Poster-Description Poster-Description";
                    prj.PosterUrl = "http://localhost/image90.png";
                    prj.ReleaseDate = new DateTime(2020, 12, 12);
                    prj.DescriptionShort = "DescriptionShort DescriptionShort";
                    prj.ExternalUrls = new List<ExternalUrl>
                    {
                        new ExternalUrl { DisplayName = "DisplayName-name", Url = "http://localhost/thing"},
                        new ExternalUrl { DisplayName = "DisplayName-name-0", Url = "http://localhost/thing0"},
                    };

                    responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    prj.Category.TotalProjects = -1;
                    prj.Version++;
                    Validator.Compare(prj, responseSaveProject.Data);
                    // *****************************

                    // Step 4: Check project (by code)
                    responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckSucceed(responseGetProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseGetProject.Data);
                    // *****************************

                    // Step 4: Check project preview
                    responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Single(responseGetProjectsPreview.Data);

                    preview = new ProjectPreview
                    {
                        Category = prj.Category,
                        Code = prj.Code,
                        Description = prj.DescriptionShort,
                        DisplayName = prj.DisplayName,
                        PosterDescription = prj.PosterDescription,
                        PosterUrl = prj.PosterUrl,
                        ReleaseDate = prj.ReleaseDate
                    };

                    Validator.Compare(preview, responseGetProjectsPreview.Data[0]);
                    prj = responseGetProject.Data;
                    // *****************************

                    // Step 6: Edit project 
                    prj.ExternalUrls = prj.ExternalUrls.Take(1).ToList();

                    responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    prj.Category.TotalProjects = -1;
                    prj.Version++;
                    prj.ExternalUrls.ToList().ForEach(x => x.Version++);
                    Validator.Compare(prj, responseSaveProject.Data);
                    // *****************************

                    // Step 4: Check project (by code)
                    responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckSucceed(responseGetProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseGetProject.Data);
                    // *****************************

                    // Step 4: Check project preview
                    responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Single(responseGetProjectsPreview.Data);

                    preview = new ProjectPreview
                    {
                        Category = prj.Category,
                        Code = prj.Code,
                        Description = prj.DescriptionShort,
                        DisplayName = prj.DisplayName,
                        PosterDescription = prj.PosterDescription,
                        PosterUrl = prj.PosterUrl,
                        ReleaseDate = prj.ReleaseDate
                    };

                    Validator.Compare(preview, responseGetProjectsPreview.Data[0]);
                    prj = responseGetProject.Data;
                    // *****************************

                    // Step 6: Edit project 
                    prj.ExternalUrls.ToList().ForEach(x => x.DisplayName += "extra");

                    responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckSucceed(responseSaveProject);
                    prj.Category.TotalProjects = -1;
                    prj.Version++;
                    prj.ExternalUrls.ToList().ForEach(x => x.Version++);
                    Validator.Compare(prj, responseSaveProject.Data);
                    // *****************************

                    // Step 4: Check project (by code)
                    responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckSucceed(responseGetProject);
                    prj.Category.TotalProjects = -1;
                    Validator.Compare(prj, responseGetProject.Data);
                    // *****************************

                    // Step 4: Check project preview
                    responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Single(responseGetProjectsPreview.Data);

                    preview = new ProjectPreview
                    {
                        Category = prj.Category,
                        Code = prj.Code,
                        Description = prj.DescriptionShort,
                        DisplayName = prj.DisplayName,
                        PosterDescription = prj.PosterDescription,
                        PosterUrl = prj.PosterUrl,
                        ReleaseDate = prj.ReleaseDate
                    };

                    Validator.Compare(preview, responseGetProjectsPreview.Data[0]);
                    prj = responseGetProject.Data;
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
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
                    new Project
                    {
                        Code = string.Empty,
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = string.Empty,
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = string.Empty,
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "string.Empty",
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        Category = new Category() {Code = "null"}
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        Category = new Category() {Code = "null"}
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        Category = new Category() {Code = "nice"}
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        Category = new Category() {Id = Guid.NewGuid(), Code = "nice"}
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl{ DisplayName = string.Empty }
                        }
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Code = "code",
                        Description = "Description",
                        DescriptionShort = "Description short",
                        DisplayName = "DisplayName",
                        ExternalUrls = new List<ExternalUrl>
                        {
                            new ExternalUrl{ DisplayName = "Display", Url = string.Empty }
                        }
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(InvalidCreate))]
        internal async Task CreateNewProject_Negative(Project project)
        {
            // Story ***********************
            // Step 1: Request projects preview from an empty database
            // Step 2: Request categories an empty database (Total projects count in system category = 0)
            // Step 3: Create new category
            // Step 3: Create new BAD project and assign it to the new category
            // Step 4: Check project (by code)
            // Step 4: Check project preview
            // Step 4: Check category
            // Step 4: Check category==all
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // Step 3: Create new category
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

                    // Step 3: Create new project and assign it to the new category
                    var prj = project;

                    if (project.Category == null)
                    {
                        prj.Category = newCategory;
                    }
                    if (project.Category?.Code == "null")
                    {
                        prj.Category = null;
                    }

                    var responseSaveProject =
                    (
                        await apiPrivate.SaveProjectAsync(prj)
                    ).Value;
                    Validator.CheckFail(responseSaveProject);
                    // *****************************

                    // Step 4: Check project (by code)
                    var responseGetProject =
                    (
                        await apiPublic.GetProjectAsync(prj.Code)
                    ).Value;
                    Validator.CheckFail(responseGetProject);
                    // *****************************

                    // Step 4: Check project preview
                    var responseGetProjectsPreview =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;
                    Validator.CheckSucceed(responseGetProjectsPreview);
                    Assert.Empty(responseGetProjectsPreview.Data);
                    // *****************************

                    // Step 4: Check category
                    var responseGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(newCategory.Id)
                    ).Value;
                    Validator.CheckSucceed(responseGetCategory);
                    Assert.Equal(0, responseGetCategory.Data.TotalProjects);
                    // *****************************

                    // Step 4: Check category==all
                    var responseGetCategoryDef =
                    (
                        await apiPublic.GetCategoryAsync(Default.Category.Id)
                    ).Value;
                    Validator.CheckSucceed(responseGetCategoryDef);
                    Assert.Equal(0, responseGetCategoryDef.Data.TotalProjects);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task EditProject_Negative()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var ethaloneCode = "code";
                    _ = await Creator.CreateNewProject(context, ethaloneCode);

                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);

                    // 
                    {
                        var failPrj =
                        (
                            await apiPublic.GetProjectAsync(ethaloneCode)
                        ).Value.Data;

                        failPrj.Id = Guid.NewGuid();
                        var responseSaveProject =
                        (
                            await apiPrivate.SaveProjectAsync(failPrj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }
                    // *****************************

                    // 
                    {
                        var failPrj =
                        (
                            await apiPublic.GetProjectAsync(ethaloneCode)
                        ).Value.Data;

                        failPrj.Code = string.Empty;
                        var responseSaveProject =
                        (
                            await apiPrivate.SaveProjectAsync(failPrj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }
                    // *****************************

                    // 
                    {
                        var failPrj =
                        (
                            await apiPublic.GetProjectAsync(ethaloneCode)
                        ).Value.Data;

                        failPrj.Version++;
                        var responseSaveProject =
                        (
                            await apiPrivate.SaveProjectAsync(failPrj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }
                    // *****************************

                    // 
                    {
                        var failPrj =
                        (
                            await apiPublic.GetProjectAsync(ethaloneCode)
                        ).Value.Data;

                        failPrj.DisplayName = string.Empty;
                        var responseSaveProject =
                        (
                            await apiPrivate.SaveProjectAsync(failPrj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }
                    // *****************************

                    // 
                    {
                        _ = await Creator.CreateNewProject(context, "ss");
                        var failPrj =
                        (
                            await apiPublic.GetProjectAsync(ethaloneCode)
                        ).Value.Data;

                        failPrj.Code = "ss";
                        var responseSaveProject =
                        (
                            await apiPrivate.SaveProjectAsync(failPrj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task Delete_Negative()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);
                    var apiPrivate = Initializer.CreatePrivateController(context);
                    var code = "code";
                    _ = await Creator.CreateNewProject(context, code);

                    {
                        var prj =
                        (
                            await apiPublic.GetProjectAsync(code)
                        ).Value.Data;

                        prj.Id = null;
                        var responseSaveProject =
                        (
                            await apiPrivate.DeleteProjectAsync(prj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }

                    {
                        var prj =
                        (
                            await apiPublic.GetProjectAsync(code)
                        ).Value.Data;

                        prj.Id = Guid.NewGuid();
                        var responseSaveProject =
                        (
                            await apiPrivate.DeleteProjectAsync(prj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
                    }

                    {
                        var prj =
                        (
                            await apiPublic.GetProjectAsync(code)
                        ).Value.Data;

                        prj.Version++;
                        var responseSaveProject =
                        (
                            await apiPrivate.DeleteProjectAsync(prj)
                        ).Value;
                        Validator.CheckFail(responseSaveProject);
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
