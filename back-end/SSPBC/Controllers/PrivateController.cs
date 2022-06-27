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
        private readonly string _tokenPrefix = "Bearer ";

        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;
        private readonly IIntroductionRepository _introductionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly Supervisor _supervisor;

        public PrivateController(IAccountRepository accountRepository,
                                 ICategoryRepository categoryRepository, 
                                 IConfiguration configuration,
                                 IFileRepository fileRepository, 
                                 IIntroductionRepository introductionRepository, 
                                 IProjectRepository projectRepository,
                                 ISessionRepository sessionRepository,
                                 Supervisor supervisor)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _fileRepository = fileRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
            _sessionRepository = sessionRepository;
            _supervisor = supervisor;
        }

        [HttpPost("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account>>> SaveAccountAsync([FromBody] Account account, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _accountRepository.SaveAsync(account);
                }
            );

            return result;
        }

        [HttpGet("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account[]>>> GetAccountsAsync([FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _accountRepository.GetAsync();
                }
            ); 

            return result;
        }

        [HttpDelete("accounts")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteAccountAsync([FromBody] Account account, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _accountRepository.DeleteAsync(account);
                }
            );

            return result;
        }

        [HttpGet("accounts/{id}")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Account>>> GetAccountAsync(Guid id, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _accountRepository.GetAsync(id);
                }
            );

            return result;
        }

        [HttpGet("roles")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<List<string>>>> GetRoles([FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);

                    return RoleNames.GetRoles();
                }
            );

            return result;
        }

        [HttpPost("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Category>>> SaveCategoryAsync([FromBody] Category category, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _categoryRepository.SaveAsync(category);
                }
            );

            return result;
        }

        [HttpDelete("category")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteCategoryAsync([FromBody] Category category, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _categoryRepository.DeleteAsync(category);
                }
            );

            return result;
        }

        [HttpPost("introduction")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Introduction>>> SaveIntroductionAsync([FromBody] Introduction introduction, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _introductionRepository.SaveAsync(introduction);
                }
            );

            return result;
        }

        [HttpPost("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<Project>>> SaveProjectAsync([FromBody] Project project, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _projectRepository.SaveAsync(project);
                }
            );

            return result;
        }

        [HttpDelete("project")]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<bool>>> DeleteProjectAsync([FromBody] Project project, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);
                    return await _projectRepository.DeleteAsync(project); 
                }
            );

            return result;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        [ApiVersion("1.0")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public async Task<ActionResult<ExecutionResult<string>>> Upload([FromForm] IFormFile file, [FromHeader] string authorization, [FromHeader] string fingerprint)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                async () =>
                {
                    var token = authorization.Split(_tokenPrefix).Last();
                    await _sessionRepository.CheckSessionAsync(token, fingerprint);

                    var fileName = _fileRepository.Save(file);
                    return Utils.AppendUrlToName(_configuration, fileName);
                }
            );

            return result;
        }
    }
}
