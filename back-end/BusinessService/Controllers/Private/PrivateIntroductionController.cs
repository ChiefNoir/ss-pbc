using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateIntroductionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;
        private readonly IIntroductionRepository _introductionRepository;

        public PrivateIntroductionController(IFileRepository fileRepository, IConfiguration configuration, IIntroductionRepository introductionRepository)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
            _introductionRepository = introductionRepository;
        }

        [HttpPatch("introduction"), DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateIntroduction([FromHeader] string token, [FromForm] Introduction introduction)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                HandleFiles(introduction, Request.Form.Files);
                return _introductionRepository.SaveAsync(introduction);
            });

            return new JsonResult(result);
        }

        private void HandleFiles(Introduction introduction, IFormFileCollection files)
        {
            var poster = files.FirstOrDefault(x => x.Name == "introduction[posterToUpload]");
            if (poster != null)
            {
                var filename = _fileRepository.Save(poster);
                introduction.PosterUrl = AppendUrlToName(filename);
            }
        }

        private string AppendUrlToName(string name)
        {
            return _configuration.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                  + "/" + _configuration.GetSection("Location:FileStorage").Get<string>()
                  + "/" + name;
        }
    }
}