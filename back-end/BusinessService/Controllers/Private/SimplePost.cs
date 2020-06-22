using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class SimplePost : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IProjectRepository _projectRepository;

        public SimplePost(ICategoryRepository categoryRepository, IProjectRepository projectRepository)
        {
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
        }


        [HttpPost("category")]
        public async Task<IActionResult> Save([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.SaveCategory(category);
            });

            return new JsonResult(result);
        }

        [HttpDelete("category")]
        public async Task<IActionResult> Delete([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.DeleteCategory(category);
            });

            return new JsonResult(result);
        }

        [HttpPost("project")]
        public async Task<IActionResult> Save([FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Save(project);
            });

            return new JsonResult(result);
        }

    }
}
