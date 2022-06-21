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
                    null
                };
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
        internal void UploadFile_Negative(FormFile form)
        {
            using (var context = Initializer.CreateDataContext())
            {
                var apiPrivate = Initializer.CreatePrivateController(context);

                var responseUpload = apiPrivate.Upload(form).Value;
                Validator.CheckFail(responseUpload);
            }
        }
    }
}
