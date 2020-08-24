using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateFileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;

        public PrivateFileController(IFileRepository fileRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromHeader] string token)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, async () =>
            {
                var filename = await _fileRepository.Save(Request.Form.Files.FirstOrDefault());

                return AppendUrlToName(filename);
            });

            return new JsonResult(result);
        }

        private string AppendUrlToName(string name)
        {
            return _configuration.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                  + "/" + _configuration.GetSection("Location:FileStorage").Get<string>()
                  + "/" + name;
        }
    }
}