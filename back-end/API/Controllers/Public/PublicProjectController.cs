using Abstractions.IRepository;
using Abstractions.Supervision;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class PublicProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISupervisor _supervisor;

        public PublicProjectController(IProjectRepository projectRepository, ISupervisor supervisor)
        {
            _projectRepository = projectRepository;
            _supervisor = supervisor;
        }

        [HttpGet("projects/{code}")]
        public async Task<IActionResult> GetProject(string code)
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetAsync(code);
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/search")]
        public async Task<IActionResult> GetProjectsPreview([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery)
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetPreviewAsync(paging.Start, paging.Length, searchQuery.CategoryCode);
            });

            return new JsonResult(result);
        }
    }
}