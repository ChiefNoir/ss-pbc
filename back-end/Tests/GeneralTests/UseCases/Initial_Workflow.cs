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
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPublic = Initializer.CreatePublicController(context, cache);

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
                       await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;

                    Validator.CheckSucceed(responseLogin);
                    Validator.Compare(Default.Account, responseLogin.Data!);

                    Assert.Equal
                    (
                        Initializer.CreateConfiguration().GetSection("Token:LifeTime").Get<int>(),
                        responseLogin.Data.TokenLifeTimeMinutes
                    );
                    Assert.NotNull(responseLogin.Data.Token);
                    //

                    //
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);
                    var responseRoles =
                    (
                        await apiPrivate.GetRoles(responseLogin!.Data.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckSucceed(responseRoles);
                    Validator.Compare(RoleNames.GetRoles(), responseRoles.Data);
                    //

                    //
                    var resultGetAccounts =
                    (
                        await apiPrivate.GetAccountsAsync(responseLogin.Data.Token, Default.Credentials.Fingerprint)
                    ).Value;

                    Validator.CheckSucceed(resultGetAccounts);
                    Validator.Compare(new[] { Default.Account }, resultGetAccounts.Data);
                    //
                }
                finally
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }

        }

    }
}
