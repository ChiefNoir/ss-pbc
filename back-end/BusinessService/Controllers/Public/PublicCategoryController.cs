using Abstractions.IRepository;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class PublicCategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupervisor _supervisor;

        public PublicCategoryController(ICategoryRepository categoryRepository, ISupervisor supervisor)
        {
            _categoryRepository = categoryRepository;
            _supervisor = supervisor;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetAsync();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetAsync(id);
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/everything")]
        public async Task<IActionResult> GetEverythingCategory()
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetTechnicalAsync();
            });

            return new JsonResult(result);
        }
    }
}