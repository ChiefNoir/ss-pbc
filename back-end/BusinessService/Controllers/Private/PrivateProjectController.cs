using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public PrivateProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost("project")]
        public async Task<IActionResult> Add([FromHeader] string token, [FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _projectRepository.Create(project);
            });

            return new JsonResult(result);
        }

        [HttpPatch("project")]
        public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _projectRepository.Update(project);
            });

            return new JsonResult(result);
        }

        [HttpDelete("project")]
        public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _projectRepository.Delete(project);
            });

            return new JsonResult(result);
        }


    }
}