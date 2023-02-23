using Microsoft.AspNetCore.Http;
using System.Collections;

namespace GeneralTests.UseCases
{
    [Trait("Category", "e2e")]
    [CollectionDefinition("database_sensitive", DisableParallelization = true)]
    public sealed class FileOperations_Workflow
    {
        private class FormFileInvalid : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new FormFile(new MemoryStream(), 0, 0, "test", "red")
                };
                yield return new object[]
                {
                    new FormFile(new MemoryStream(), 0, int.MaxValue, "test", "red")
                };
            }
        }


        [Theory]
        [ClassData(typeof(FormFileInvalid))]
        internal async Task UploadFile_NegativeAsync(FormFile form)
        {
            var (context, cache) = Initializer.CreateDataContext();
            using (context)
            {
                try
                {
                    context.Migrator.MigrateUp();
                    var apiPrivate = Initializer.CreatePrivateController(context, cache);
                    var apiGateway = Initializer.CreateGatewayController(context);
                    var resultLogin =
                    (
                        await apiGateway.LoginAsync(Default.Credentials)
                    ).Value;

                    var responseUpload =
                    (
                        await apiPrivate.Upload(form, resultLogin!.Data!.Token, Default.Credentials.Fingerprint)
                    ).Value;
                    Validator.CheckFail(responseUpload);
                }
                catch
                {
                    context.Migrator.MigrateDown(0); await cache.FlushAsync();
                }
            }
        }
    }
}
