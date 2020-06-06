using Abstractions.IRepository;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessService.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class SimpleGet : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly INewsRepository _newsRepository;

        public SimpleGet(ICategoryRepository categoryRepository, IProjectRepository projectRepository, INewsRepository newsRepository)
        {
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
            _newsRepository = newsRepository;
        }

        [HttpGet("news/all")]
        public async Task<IActionResult> GetNews()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _newsRepository.GetNews();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/all")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategories();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/count")]
        public async Task<IActionResult> GetProjectsTotal()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Count();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/count")]
        public async Task<IActionResult> GetProjectsTotal(string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Count(categoryCode);
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/{start}/{length}/")]
        public async Task<IActionResult> GetProjects(int start, int length, string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProjects(start, length, categoryCode);

            });

            return new JsonResult(result);
        }

        [HttpGet("project/{code}")]
        public async Task<IActionResult> GetProject(string code)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProject(code);
            });

            return new JsonResult(result);
        }
    }
}
