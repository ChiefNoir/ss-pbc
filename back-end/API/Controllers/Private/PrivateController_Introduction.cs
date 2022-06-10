using Abstractions.API;
using Abstractions.Model;
using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override async Task<IActionResult> SaveIntroductionAsync([FromBody] Introduction introduction)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    // TODO: 
                    // HandleFiles(introduction, Request?.Form?.Files);
                    return _introductionRepository.SaveAsync(introduction);
                }
            );

            return new JsonResult(result);
        }

        private void HandleFiles(Introduction introduction, IFormFileCollection files)
        {
            if (files == null || !files.Any())
            {
                return;
            }

            var poster = files.FirstOrDefault(x => x.Name == "introduction[posterToUpload]");
            if (poster != null)
            {
                var filename = _fileRepository.Save(poster);
                introduction.PosterUrl = Utils.AppendUrlToName(_configuration, filename);
            }
        }

    }
}