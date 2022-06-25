using Abstractions.IRepositories;
using Abstractions.Models;
using Abstractions.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using Security.Models;
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

        public PrivateController(IAccountRepository accountRepository,
                                 ICategoryRepository categoryRepository, 
                                 IConfiguration configuration,
                                 IFileRepository fileRepository, 
                                 IIntroductionRepository introductionRepository, 
                                 IProjectRepository projectRepository, 
                                 Supervisor supervisor)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _fileRepository = fileRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
            _supervisor = supervisor;
        }

        [HttpPost("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account>>> SaveAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SaveAsync(account)
            );

            return result;
        }

        [HttpGet("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account[]>>> GetAccountsAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync()
            ); ;

            return result;
        }

        [HttpDelete("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.DeleteAsync(account)
            );

            return result;
        }

        [HttpGet("accounts/{id}")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account>>> GetAccountAsync(Guid id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync(id)
            );

            return result;
        }

        [HttpGet("roles")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public ActionResult<ExecutionResult<List<string>>> GetRoles()
        {
            var result = _supervisor.SafeExecute
            (
                () =>
                {
                    return RoleNames.GetRoles();
                }
            );

            return result;
        }

        [HttpPost("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Category>>> SaveCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.SaveAsync(category)
            );

            return result;
        }

        [HttpDelete("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.DeleteAsync(category)
            );

            return result;
        }

        [HttpPost("introduction")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Introduction>>> SaveIntroductionAsync([FromBody] Introduction introduction)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    return _introductionRepository.SaveAsync(introduction);
                }
            );

            return result;
        }

        [HttpPost("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Project>>> SaveProjectAsync([FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () =>
                {
                    return _projectRepository.SaveAsync(project);
                }
            );

            return result;
        }

        [HttpDelete("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteProjectAsync([FromBody] Project project)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _projectRepository.DeleteAsync(project)
            );

            return result;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public ActionResult<ExecutionResult<string>> Upload([FromForm] IFormFile file)
        {
            var result = _supervisor.SafeExecute
            (
                () =>
                {
                    var fileName = _fileRepository.Save(file);

                    return Utils.AppendUrlToName(_configuration, fileName);
                }
            );

            return result;
        }
    }
}
