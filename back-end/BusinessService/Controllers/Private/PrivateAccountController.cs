using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateAccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public PrivateAccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> AddAccount([FromHeader] string token, [FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[]{ RoleNames.Admin }, () =>
            {
                return _accountRepository.Add(account);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> CountAccounts([FromHeader] string token)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _accountRepository.Count();
            });

            return new JsonResult(result);
        }

        [HttpDelete("accounts")]
        public async Task<IActionResult> DeleteUser([FromHeader] string token, [FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _accountRepository.Remove(account);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts/{id}")]
        public async Task<IActionResult> GetAccount([FromHeader] string token, int id)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _accountRepository.Get(id);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts/search")]
        public async Task<IActionResult> GetAccounts([FromHeader] string token, [FromQuery] Paging paging)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _accountRepository.Search(paging.Start, paging.Length);
            });

            return new JsonResult(result);
        }

        [HttpPatch("accounts")]
        public async Task<IActionResult> UpdateAccount([FromHeader] string token, [FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _accountRepository.Update(account);
            });

            return new JsonResult(result);
        }
    }
}