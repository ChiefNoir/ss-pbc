using Abstractions.Models;
using Infrastructure;
using SSPBC.Models;

namespace GeneralTests
{
    internal static class Creator
    {
        private static readonly string newCategoryCode = "cute";

        public static async Task<Project> CreateNewProject(DataContext context, string code, string? categoryCode = null)
        {
            var apiPublic = Initializer.CreatePublicController(context);
            var apiPrivate = Initializer.CreatePrivateController(context);

            // Step 1: Create/get category
            var category = await CreateNewCategory(context, categoryCode);
            // *****************************

            // Step 2: Create new project and assign it to the new category
            var prj = new Project
            {
                Code = code,
                DisplayName = "name",
                Description = "Description",
                PosterDescription = "Poster-Description",
                PosterUrl = "http://localhost/image.png",
                ReleaseDate = new DateTime(2000, 12, 12),
                DescriptionShort = "descr short",
                Category = category,
                ExternalUrls = new List<ExternalUrl>
                {
                    new ExternalUrl{ DisplayName = "ExternalUrl-DisplayName-0", Url = "ExternalUrl-Url-0"}
                }
            };
            var responseSaveProject =
            (
                await apiPrivate.SaveProjectAsync(prj)
            ).Value;
            Validator.CheckSucceed(responseSaveProject);
            Validator.Compare(prj, responseSaveProject.Data);
            // *****************************

            // Step 3: Check project (by code)
            var responseGetProject =
            (
                await apiPublic.GetProjectAsync(prj.Code)
            ).Value;
            Validator.CheckSucceed(responseGetProject);
            Validator.Compare(prj, responseGetProject.Data);
            // *****************************

            // Step 4: Check project preview
            var responseGetProjectsPreview =
            (
                await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
            ).Value;
            Validator.CheckSucceed(responseGetProjectsPreview);
            Assert.Equal(category.TotalProjects + 1, responseGetProjectsPreview.Data.Length);

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

            Validator.Compare(preview, responseGetProjectsPreview.Data.First(x => x.Code == prj.Code));
            // *****************************

            // Step 5: Check category
            var responseGetCategory =
            (
                await apiPublic.GetCategoryAsync(category.Id)
            ).Value;
            Validator.CheckSucceed(responseGetCategory);
            Assert.Equal(category.TotalProjects + 1, responseGetCategory.Data.TotalProjects);
            // *****************************

            // Step 6: Check category==all
            var responseGetCategoryDef =
            (
                await apiPublic.GetCategoryAsync(Default.Category.Id)
            ).Value;
            Validator.CheckSucceed(responseGetCategoryDef);
            Assert.Equal(category.TotalProjects + 1, responseGetCategoryDef.Data.TotalProjects);
            // *****************************

            var result = await apiPublic.GetProjectAsync(prj.Code);
            return result.Value.Data;
        }

        public static async Task<Category> CreateNewCategory(DataContext context, string? categoryCode = null)
        {
            var apiPublic = Initializer.CreatePublicController(context);
            var apiPrivate = Initializer.CreatePrivateController(context);

            var responseGetCategories =
            (
                await apiPublic.GetCategoriesAsync()
            ).Value;
            Validator.CheckSucceed(responseGetCategories);
            var category = responseGetCategories.Data.FirstOrDefault(x => x.Code == (categoryCode ?? newCategoryCode));

            if (category == null)
            {
                //Step 1: Create a new category
                category = new Category
                {
                    Code = (categoryCode ?? newCategoryCode),
                    DisplayName = "Cute Display Name",
                    IsEverything = false,
                    Id = null
                };

                var responseSaveCategory =
                (
                    await apiPrivate.SaveCategoryAsync(category)
                ).Value;
                Validator.CheckSucceed(responseSaveCategory);
                Validator.Compare(category, responseSaveCategory.Data);
                category = responseSaveCategory.Data;
                // *****************************
            }

            //Step 2: Check if category exist
            responseGetCategories =
            (
                await apiPublic.GetCategoriesAsync()
            ).Value;
            Validator.CheckSucceed(responseGetCategories);

            var existingCategory = responseGetCategories.Data.First(x => x.Code == category.Code);
            Validator.Compare(category, existingCategory);
            // *****************************

            return category;
        }
    }
}
