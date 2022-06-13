using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public abstract Task<IActionResult> SaveIntroductionAsync([FromBody] Introduction introduction);

        [HttpPost("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveAccountAsync([FromBody] Account account);

        [HttpGet("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> GetAccountsAsync();

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

        [HttpPost("project")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveProjectAsync([FromBody] Project project);

        [HttpDelete("project")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteProjectAsync([FromBody] Project project);

        [HttpPost("upload"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract IActionResult Upload([FromForm] IFormFile File);
    }
}
