using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.Model.Queries;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    public partial class PublicController : PublicControllerBase
    {
        public override async Task<IActionResult> GetProjectAsync(string code)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.GetAsync(code)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetProjectsPreviewAsync([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.GetPreviewAsync(paging.Start, paging.Length, searchQuery.CategoryCode)
            );

            return new JsonResult(result);
        }
    }
}