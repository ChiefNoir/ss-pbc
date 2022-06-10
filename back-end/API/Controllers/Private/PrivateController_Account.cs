using Abstractions.API;
using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override async Task<IActionResult> SaveAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SaveAsync(account)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetAccountsAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync()
            );;

            return new JsonResult(result);
        }

        public override async Task<IActionResult> DeleteAccountAsync([FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.DeleteAsync(account)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetAccountAsync(int id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetAccountsAsync([FromQuery] Paging paging)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SearchAsync(paging.Start, paging.Length)
            );

            return new JsonResult(result);
        }

        public override IActionResult GetRoles()
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

    }
}