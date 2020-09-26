using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateCategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupervisor _supervisor;

        public PrivateCategoryController(ICategoryRepository categoryRepository, ISupervisor supervisor)
        {
            _categoryRepository = categoryRepository;
            _supervisor = supervisor;
        }

        [HttpPost("category")]
        public async Task<IActionResult> Save([FromHeader] string token, [FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _categoryRepository.SaveAsync(category);
            });

            return new JsonResult(result);
        }

        [HttpDelete("category")]
        public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _categoryRepository.DeleteAsync(category);
            });

            return new JsonResult(result);
        }
    }
}