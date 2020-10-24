using Abstractions.API;
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
    public partial class PrivateController : PrivateControllerBase
    {
        public override async Task<IActionResult> SaveProjectAsync([FromForm] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    HandleFiles(project, Request?.Form?.Files);
                    return _projectRepository.SaveAsync(project);
                }
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> DeleteProjectAsync([FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.DeleteAsync(project)
            );

            return new JsonResult(result);
        }

        private void HandleFiles(Project project, IFormFileCollection files)
        {
            if (files == null || !files.Any())
                return;

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