using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override async Task<IActionResult> SaveAccountAsync([FromHeader] string authorization, [FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SaveAsync(account)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> CountAccountAsync([FromHeader] string authorization)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.CountAsync()
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> DeleteAccountAsync([FromHeader] string authorization, [FromBody] Account account)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.DeleteAsync(account)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetAccountAsync([FromHeader] string authorization, int id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetAccountsAsync([FromHeader] string authorization, [FromQuery] Paging paging)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _accountRepository.SearchAsync(paging.Start, paging.Length)
            );

            return new JsonResult(result);
        }

        public override IActionResult GetRoles([FromHeader] string authorization)
        {
            var result = _supervisor.SafeExecute
            (
                () =>
                {
                    var lst = new List<string>();
                    foreach (var property in typeof(RoleNames).GetProperties())
                    {
                        lst.Add(property.GetValue(null, null)?.ToString());
                    }

                    return lst;
                }
            );

            return new JsonResult(result);
        }

    }
}