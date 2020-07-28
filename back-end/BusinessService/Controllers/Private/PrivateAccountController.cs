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
    public class PrivateAccountController
    {
        private readonly IAccountRepository _accountRepository;

        public PrivateAccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> AddUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Add(account);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> CountAccounts()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Count();
            });

            return new JsonResult(result);
        }

        [HttpDelete("accounts")]
        public async Task<IActionResult> DeleteUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Remove(account);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Get(id);
            });

            return new JsonResult(result);
        }

        [HttpGet("accounts/search")]
        public async Task<IActionResult> GetUsers([FromQuery] Paging paging)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Search(paging.Start, paging.Length);
            });

            return new JsonResult(result);
        }

        [HttpPatch("accounts")]
        public async Task<IActionResult> UpdateUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Update(account);
            });

            return new JsonResult(result);
        }
    }
}