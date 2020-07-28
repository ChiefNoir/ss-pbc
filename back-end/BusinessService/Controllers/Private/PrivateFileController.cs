using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateFileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;

        public PrivateFileController(IFileRepository fileRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var result = await Supervisor.SafeExecuteAsync(async () =>
            {
                var filename = await _fileRepository.Save(Request.Form.Files.FirstOrDefault());

                return FormatToUrl(filename);
            });

            return new JsonResult(result);
        }

        private string FormatToUrl(string name)
        {
            return
            _configuration.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
            + "/" +
            _configuration.GetSection("Location:FileStorage").Get<string>()
            + "/"
            + name;
        }

    }
}
