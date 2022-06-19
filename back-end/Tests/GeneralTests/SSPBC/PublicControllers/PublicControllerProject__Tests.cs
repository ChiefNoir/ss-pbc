using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Security.Models;

namespace GeneralTests.SSPBC.PublicControllers
{
    [Collection("database_sensitive")]
    public class PublicControllerProject__Tests
    {
        [Fact]
        internal async Task GetProjectsAsync__Valid()
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePublicController(context);

                    var response =
                    (
                        (JsonResult)await api.GetProjectAsync("code")
                    ).Value as ExecutionResult<Project>;

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
