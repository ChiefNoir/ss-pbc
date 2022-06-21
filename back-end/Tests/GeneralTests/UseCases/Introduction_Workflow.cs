using Abstractions.Models;

namespace GeneralTests.UseCases
{
    [Collection("database_sensitive")]
    [Trait("Category", "Work-flow")]
    public sealed class Introduction_Workflow
    {
        [Fact]
        internal async Task CheckDefault_Positive()
        {
            // Story ***********************
            // Step 1: Request introduction
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var apiPublic = Initializer.CreatePublicController(context);

                    // Step 1: Request introduction
                    var publiGetIntroduction =
                    (
                        await apiPublic.GetIntroductionAsync()
                    ).Value;

                    Validator.CheckSucceed(publiGetIntroduction);
                    Validator.Compare(Default.Introduction, publiGetIntroduction.Data);
                    // *****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task Update_Positive()
        {
            // Story ***********************
            // Step 1: Request introduction
            // Step 2: Edit and save
            // Step 3: Request introduction
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Request introduction
                    var apiPublic = Initializer.CreatePublicController(context);
                    var responseIntroduction =
                    (
                        await apiPublic.GetIntroductionAsync()
                    ).Value;

                    Validator.CheckSucceed(responseIntroduction);
                    Validator.Compare(Default.Introduction, responseIntroduction.Data);
                    // ****************************

                    // Step 2: Edit basic and save
                    var newIntroduction = responseIntroduction.Data;
                    newIntroduction.Title = "Brand new title";
                    newIntroduction.Content = "Brand new content";
                    newIntroduction.PosterDescription = "Brand new poster description";
                    newIntroduction.PosterUrl = "http://localhost/image.png";
                    newIntroduction.ExternalUrls = new List<ExternalUrl>
                    {
                        new() { DisplayName = "ExternalUrl-DisplayName-0", Url = "http://localhost/ExternalUrl-URL-0" },
                        new() { DisplayName = "ExternalUrl-DisplayName-1", Url = "http://localhost/ExternalUrl-URL-1" },
                        new() { DisplayName = "ExternalUrl-DisplayName-2", Url = "http://localhost/ExternalUrl-URL-2" },
                    };

                    var apiPrivate = Initializer.CreatePrivateController(context);
                    var responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);

                    newIntroduction.Version++;
                    Validator.Compare(newIntroduction, responseSaveIntroduction.Data);
                    newIntroduction = responseSaveIntroduction.Data;
                    // ****************************

                    // Step 3: Request introduction
                    responseIntroduction =
                    (
                        await apiPublic.GetIntroductionAsync()
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);
                    Validator.Compare(newIntroduction, responseIntroduction.Data);
                    // ****************************

                    //Extra
                    // Step 2: Edit basic and save
                    newIntroduction.ExternalUrls = newIntroduction.ExternalUrls.Take(2);

                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);

                    newIntroduction.Version++;
                    newIntroduction.ExternalUrls.ToList().ForEach(x => x.Version++);
                    Validator.Compare(newIntroduction, responseSaveIntroduction.Data);
                    newIntroduction = responseSaveIntroduction.Data;
                    // --

                    newIntroduction.ExternalUrls.ToList().ForEach(x => x.DisplayName += "-extra");

                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);

                    newIntroduction.Version++;
                    newIntroduction.ExternalUrls.ToList().ForEach(x => x.Version++);
                    Validator.Compare(newIntroduction, responseSaveIntroduction.Data);
                    newIntroduction = responseSaveIntroduction.Data;
                    // --
                    newIntroduction.ExternalUrls = new List<ExternalUrl>();

                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);

                    newIntroduction.Version++;
                    Validator.Compare(newIntroduction, responseSaveIntroduction.Data);
                    newIntroduction = responseSaveIntroduction.Data;
                    // --

                    // ****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }

        [Fact]
        internal async Task Update_Negative()
        {
            // Story ***********************
            // Step 1: Request introduction
            // Step 2: Edit and save
            // Step 3: Edit, wrong version
            // *****************************

            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    // Step 1: Request introduction
                    var apiPublic = Initializer.CreatePublicController(context);
                    var responseIntroduction =
                    (
                        await apiPublic.GetIntroductionAsync()
                    ).Value;

                    Validator.CheckSucceed(responseIntroduction);
                    Validator.Compare(Default.Introduction, responseIntroduction.Data);
                    // ****************************

                    // Step 2: Edit and save
                    var newIntroduction = responseIntroduction.Data;
                    newIntroduction.Title = "Brand new title";
                    newIntroduction.Content = "Brand new content";
                    newIntroduction.PosterDescription = "Brand new poster description";
                    newIntroduction.PosterUrl = "http://localhost/image.png";
                    newIntroduction.ExternalUrls = new List<ExternalUrl>
                    {
                        new() { DisplayName = "ExternalUrl-DisplayName-0", Url = "http://localhost/ExternalUrl-URL-0" },
                        new() { DisplayName = "ExternalUrl-DisplayName-1", Url = "http://localhost/ExternalUrl-URL-1" },
                        new() { DisplayName = "ExternalUrl-DisplayName-2", Url = "http://localhost/ExternalUrl-URL-2" },
                    };

                    var apiPrivate = Initializer.CreatePrivateController(context);
                    var responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckSucceed(responseSaveIntroduction);

                    newIntroduction.Version++;
                    Validator.Compare(newIntroduction, responseSaveIntroduction.Data);
                    newIntroduction = responseSaveIntroduction.Data;
                    // ****************************

                    // Step 3: Edit, wrong version
                    newIntroduction.Version = 19;
                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckFail(responseSaveIntroduction);

                    // Step 3: Edit, wrong version
                    newIntroduction.Version = 1;
                    newIntroduction.ExternalUrls.ToList().ForEach(x => x.Version = 19);
                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckFail(responseSaveIntroduction);
                    // ****************************

                    // Step 3: Edit, wrong version
                    newIntroduction.Version = 1;
                    newIntroduction.ExternalUrls.ToList().ForEach(x => x.Version = 1);
                    newIntroduction.ExternalUrls.Last().Version = 19;
                    responseSaveIntroduction =
                    (
                        await apiPrivate.SaveIntroductionAsync(newIntroduction)
                    ).Value;
                    Validator.CheckFail(responseSaveIntroduction);
                    // ****************************
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
