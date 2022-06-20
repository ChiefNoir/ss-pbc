using Abstractions.IRepositories;
using Abstractions.Models;
using Abstractions.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using SSPBC.Helpers;

namespace SSPBC.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class PrivateController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;
        private readonly IIntroductionRepository _introductionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly Supervisor _supervisor;
        private readonly ITokenManager _tokenManager;

        public PrivateController(IAccountRepository accountRepository, ICategoryRepository categoryRepository, IConfiguration configuration, IFileRepository fileRepository, IIntroductionRepository introductionRepository, IProjectRepository projectRepository, Supervisor supervisor, ITokenManager tokenManager)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _fileRepository = fileRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
        }

        [HttpPost("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> SaveAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SaveAsync(account)
            );

            return new JsonResult(result);
        }

        [HttpGet("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync()
            ); ;

            return new JsonResult(result);
        }

        [HttpDelete("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.DeleteAsync(account)
            );

            return new JsonResult(result);
        }

        [HttpGet("accounts/{id}")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> GetAccountAsync(Guid id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }

        [HttpGet("roles")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public IActionResult GetRoles()
        {
            var result = _supervisor.SafeExecute
            (
                () =>
                {
                    return RoleNames.GetRoles();
                }
            );

            return new JsonResult(result);
        }

        [HttpPost("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> SaveCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.SaveAsync(category)
            );

            return new JsonResult(result);
        }

        [HttpDelete("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> DeleteCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.DeleteAsync(category)
            );

            return new JsonResult(result);
        }

        [HttpPost("introduction")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> SaveIntroductionAsync([FromBody] Introduction introduction)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    return _introductionRepository.SaveAsync(introduction);
                }
            );

            return new JsonResult(result);
        }

        [HttpPost("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> SaveProjectAsync([FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    return _projectRepository.SaveAsync(project);
                }
            );

            return new JsonResult(result);
        }

        [HttpDelete("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<IActionResult> DeleteProjectAsync([FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.DeleteAsync(project)
            );

            return new JsonResult(result);
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            var result = _supervisor.SafeExecute
            (
                () =>
                {
                    var fileName = _fileRepository.Save(file);

                    return Utils.AppendUrlToName(_configuration, fileName);
                }
            );

            return new JsonResult(result);
        }
    }
}
