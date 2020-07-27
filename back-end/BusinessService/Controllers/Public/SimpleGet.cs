using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
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
        private readonly IIntroductionRepository _introductionRepository;

        public SimpleGet(ICategoryRepository categoryRepository, IProjectRepository projectRepository, IIntroductionRepository introductionRepository)
        {
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
            _introductionRepository = introductionRepository;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategories();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/everything")]
        public async Task<IActionResult> GetEverythingCategory()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetEverythingCategory();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategory(id);
            });

            return new JsonResult(result);
        }


        [HttpGet("introduction")]
        public async Task<IActionResult> GetIntroduction()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _introductionRepository.GetIntroduction();
            });

            return new JsonResult(result);
        }


        [HttpGet("projects/{code}")]
        public async Task<IActionResult> GetProject(string code)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Read(code);
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/search")]
        public async Task<IActionResult> GetProjectsPreview([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProjectsPreview(paging.Start, paging.Length, searchQuery.CategoryCode);

            });

            return new JsonResult(result);
        }

    }
}
