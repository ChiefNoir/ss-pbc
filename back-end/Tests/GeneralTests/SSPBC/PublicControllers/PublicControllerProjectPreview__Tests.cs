using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using SSPBC.Models;

namespace GeneralTests.SSPBC.PublicControllers
{
    [Collection("database_sensitive")]
    public sealed class PublicControllerProjectPreview__Tests
    {
        [Fact]
        internal async Task GetProjectsPreviewAsync()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePublicController(context);

                    var response =
                    (
                        (JsonResult)await api.GetProjectsPreviewAsync(new Paging(0, 200), new ProjectSearch())
                    ).Value as ExecutionResult<ProjectPreview[]>;

                    Validator.CheckSucceed(response!);
                    Validator.Compare(Enumerable.Empty<ProjectPreview>(), response!.Data!);
                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
