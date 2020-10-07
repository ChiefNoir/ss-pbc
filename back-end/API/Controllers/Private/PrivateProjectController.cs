using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Supervision;
using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateProjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISupervisor _supervisor;

        public PrivateProjectController(IConfiguration configuration, IFileRepository fileRepository, IProjectRepository projectRepository, ISupervisor supervisor)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
            _projectRepository = projectRepository;
            _supervisor = supervisor;
        }

        [HttpPost("project"), DisableRequestSizeLimit]
        public async Task<IActionResult> Save([FromHeader] string token, [FromForm] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                HandleFiles(project, Request.Form.Files);
                return _projectRepository.SaveAsync(project);
            });

            return new JsonResult(result);
        }

        [HttpPatch("project"), DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromHeader] string token, [FromForm] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                HandleFiles(project, Request.Form.Files);
                return _projectRepository.SaveAsync(project);
            });

            return new JsonResult(result);
        }

        [HttpDelete("project"), DisableRequestSizeLimit]
        public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token, 
                new[] { RoleNames.Admin },
                () => _projectRepository.DeleteAsync(project)
            );

            return new JsonResult(result);
        }

        private void HandleFiles(Project project, IFormFileCollection files)
        {
            var poster = files.FirstOrDefault(x => x.Name == "project[posterToUpload]");
            if (poster != null)
            {
                var filename = _fileRepository.Save(poster);
                project.PosterUrl = Utils.AppendUrlToName(_configuration, filename);
            }

            var gallery = files.Where(x => x.Name.StartsWith("project[galleryImages]")).OrderBy(x => x.Name);
            foreach (var item in gallery)
            {
                var filename = _fileRepository.Save(item);
                var index = ParseIndex(item.Name);
                if (index == -1)
                    continue;

                project.GalleryImages[index].ImageUrl = Utils.AppendUrlToName(_configuration, filename);
            }
        }

        private static int ParseIndex(string galleryImagesName)
        {
            // project[galleryImages][0][readyToUpload]
            var matches = Regex.Match(galleryImagesName, @"(?<=\[)[0-9].*?(?=\])");

            if (matches.Success)
            {
                var match = matches.Captures.FirstOrDefault()?.Value;
                if (match == null)
                    return -1;

                return Convert.ToInt32(match);
            }

            return -1;
        }
    }
}