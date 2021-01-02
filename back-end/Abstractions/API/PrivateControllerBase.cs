using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Abstractions.API
{
    [ApiController]
    [Route("api/v1/")]
    public abstract class PrivateControllerBase : ControllerBase
    {
        [HttpPost("introduction"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveIntroductionAsync([FromForm] Introduction introduction);

        [HttpPost("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveAccountAsync([FromBody] Account account);

        [HttpGet("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> CountAccountAsync();

        [HttpDelete("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteAccountAsync([FromBody] Account account);

        [HttpGet("accounts/{id}")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> GetAccountAsync(int id);

        [HttpGet("accounts/search")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> GetAccountsAsync([FromQuery] Paging paging);


        [HttpGet("roles")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract IActionResult GetRoles();

        [HttpPost("category")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveCategoryAsync([FromBody] Category category);

        [HttpDelete("category")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteCategoryAsync([FromBody] Category category);

        [HttpGet("information")]
        [Authorize(Roles = Restrictions.AuthorizedRoles)]
        public abstract IActionResult GetInformationAsync();

        [HttpPost("project"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveProjectAsync([FromForm] Project project);

        [HttpDelete("project"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteProjectAsync([FromBody] Project project);

    }
}
