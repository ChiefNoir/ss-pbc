using Abstractions.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Abstractions.API
{
    [ApiController]
    [Route("api/v1/")]
    public abstract class PublicControllerBase : ControllerBase
    {
        [HttpGet("ping")]
        public abstract IActionResult Ping();

        [HttpGet("categories")]
        public abstract Task<IActionResult> GetCategoriesAsync();

        [HttpGet("categories/{id}")]
        public abstract Task<IActionResult> GetCategoryAsync(int id);

        [HttpGet("categories/everything")]
        public abstract Task<IActionResult> GetCategoryEverythingAsync();

        [HttpGet("introduction")]
        public abstract Task<IActionResult> GetIntroductionAsync();

        [HttpGet("projects/{code}")]
        public abstract Task<IActionResult> GetProjectAsync(string code);

        [HttpGet("projects/search")]
        public abstract Task<IActionResult> GetProjectsPreviewAsync([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery);
    }
}
