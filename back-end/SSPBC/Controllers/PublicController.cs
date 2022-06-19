using Abstractions.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using SSPBC.Models;

namespace SSPBC.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class PublicController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIntroductionRepository _introductionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly Supervisor _supervisor;

        public PublicController(Supervisor supervisor, ICategoryRepository categoryRepository, IIntroductionRepository introductionRepository, IProjectRepository projectRepository)
        {
            _supervisor = supervisor;
            _categoryRepository = categoryRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
        }

        
        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.GetAsync()
            );

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryAsync(Guid? id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet("introduction")]
        public async Task<IActionResult> GetIntroductionAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _introductionRepository.GetAsync()
            );

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet("projects/{code}")]
        public async Task<IActionResult> GetProjectAsync(string code)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.GetAsync(code)
            );

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpGet("projects/search")]
        public async Task<IActionResult> GetProjectsPreviewAsync([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.GetPreviewAsync(paging.Start, paging.Length, searchQuery.CategoryCode)
            );

            return new JsonResult(result);
        }
    }
}
