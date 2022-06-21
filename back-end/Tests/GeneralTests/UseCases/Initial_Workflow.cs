using Abstractions.Models;
using Abstractions.Security;
using Microsoft.Extensions.Configuration;
using SSPBC.Models;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class Initial_Workflow
    {
        [Fact]
        internal async Task LookAt_CleanDatabase()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context);

                    //
                    var publiGetIntroduction =
                    (
                        await apiPublic.GetIntroductionAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetIntroduction);
                    Validator.Compare(Default.Introduction, publiGetIntroduction.Data);
                    //

                    //
                    var publiGetCategories =
                    (
                        await apiPublic.GetCategoriesAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetCategories);
                    Validator.Compare(new[] { Default.Category }, publiGetCategories.Data);
                    //

                    //
                    var publiGetCategory =
                    (
                       await apiPublic.GetCategoryAsync(Default.Category.Id)
                    ).Value;

                    Validator.CheckSucceed(publiGetCategory);
                    Validator.Compare(Default.Category, publiGetCategory.Data);
                    //

                    //
                    var publiGetProjectsPreviewAsync =
                    (
                        await apiPublic.GetProjectsPreviewAsync(new Paging { Start = 0, Length = 100 }, new ProjectSearch { CategoryCode = null })
                    ).Value;

                    Validator.CheckSucceed(publiGetProjectsPreviewAsync);
                    Validator.Compare(Enumerable.Empty<ProjectPreview>(), publiGetProjectsPreviewAsync.Data);
                    //

                    //
                    var apiGateway = Initializer.CreateGatewayController(context);

                    var responseLogin =
                    (
                       await apiGateway.LoginAsync(Default.Account.Login, Default.Account.Password)
                    ).Value;

                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data.Account);

                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        responseLogin.Data.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(responseLogin.Data.Token);
                    //

                    //
                    var apiPrivate = Initializer.CreatePrivateController(context);
                    var responseRoles =
                    (
                        apiPrivate.GetRoles()
                    ).Value;

                    Validator.CheckSucceed(responseRoles);
                    Validator.Compare(RoleNames.GetRoles(), responseRoles.Data);
                    //

                    //
                    var resultGetAccounts =
                    (
                        await apiPrivate.GetAccountsAsync()
                    ).Value;

                    Validator.CheckSucceed(resultGetAccounts);
                    Validator.Compare(new[] { Default.Account }, resultGetAccounts.Data);
                    //
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }

        }

    }
}
