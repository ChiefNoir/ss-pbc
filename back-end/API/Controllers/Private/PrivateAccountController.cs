using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Supervision;
using API.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateAccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISupervisor _supervisor;

        public PrivateAccountController(IAccountRepository accountRepository, ISupervisor supervisor)
        {
            _accountRepository = accountRepository;
            _supervisor = supervisor;
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> SaveAsync([FromHeader] string token, [FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token,
                new[] { RoleNames.Admin }, 
                () => _accountRepository.SaveAsync(account)
            );

            return new JsonResult(result);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> CountAsync([FromHeader] string token)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token, 
                new[] { RoleNames.Admin }, 
                () => _accountRepository.CountAsync()
            );

            return new JsonResult(result);
        }

        [HttpDelete("accounts")]
        public async Task<IActionResult> DeleteAsync([FromHeader] string token, [FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token, 
                new[] { RoleNames.Admin },
                () => _accountRepository.DeleteAsync(account)
            );

            return new JsonResult(result);
        }

        [HttpGet("accounts/{id}")]
        public async Task<IActionResult> GetAsync([FromHeader] string token, int id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token, 
                new[] { RoleNames.Admin }, 
                () => _accountRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }

        [HttpGet("accounts/search")]
        public async Task<IActionResult> GetAsync([FromHeader] string token, [FromQuery] Paging paging)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                token, 
                new[] { RoleNames.Admin }, 
                () => _accountRepository.SearchAsync(paging.Start, paging.Length)
            );

            return new JsonResult(result);
        }

        [HttpGet("roles")]
        public IActionResult GetRoles([FromHeader] string token)
        {
            var result = _supervisor.SafeExecute(token, new[] { RoleNames.Admin }, () =>
            {
                var lst = new List<string>();
                foreach (var property in typeof(RoleNames).GetProperties())
                {
                    lst.Add(property.GetValue(null, null)?.ToString());
                }

                return lst;
            });

            return new JsonResult(result);
        }

    }
}