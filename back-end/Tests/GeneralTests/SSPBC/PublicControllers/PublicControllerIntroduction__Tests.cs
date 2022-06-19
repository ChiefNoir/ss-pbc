using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using System.Collections;

namespace GeneralTests.SSPBC.PublicControllers
{
    [Collection("database_sensitive")]
    public class PublicControllerIntroduction__Tests
    {
        class DefaultIntroduction_EmptyDatabase : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Introduction
                    {
                        Title = "Hello",
                        Content = "The service is on-line. Congratulations.",
                        Version = 0,
                        ExternalUrls = new List<ExternalUrl>()
                    }
                };
            }
        }

        [Theory]
        [ClassData(typeof(DefaultIntroduction_EmptyDatabase))]
        internal async Task GetIntroductionAsync(Introduction expected)
        {
            using (var context = Initializer.CreateDataContext())
            {
                try
                {
                    context.Migrator.MigrateUp();

                    var api = Initializer.CreatePublicController(context);

                    var response =
                    (
                        (JsonResult)await api.GetIntroductionAsync()
                    ).Value as ExecutionResult<Introduction>;

                    Validator.CheckSucceed(response);
                    Validator.Compare(expected, response!.Data!);

                }
                finally
                {
                    context.Migrator.MigrateDown(0);
                }
            }
        }
    }
}
