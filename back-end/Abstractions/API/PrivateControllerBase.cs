using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Abstractions.API
{
    [ApiController]
    [Route("api/v1/")]
    public abstract class PrivateControllerBase : ControllerBase
    {
        //TODO: replace [FromForm] to [FromBody], change rile handling
        [HttpPost("introduction"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveIntroductionAsync([FromHeader] string authorization, [FromForm] Introduction introduction);

        [HttpPost("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveAccountAsync([FromHeader] string authorization, [FromBody] Account account);

        [HttpGet("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> CountAccountAsync([FromHeader] string authorization);

        [HttpDelete("accounts")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteAccountAsync([FromHeader] string authorization, [FromBody] Account account);

        [HttpGet("accounts/{id}")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> GetAccountAsync([FromHeader] string authorization, int id);

        [HttpGet("accounts/search")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> GetAccountsAsync([FromHeader] string authorization, [FromQuery] Paging paging);


        [HttpGet("roles")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract IActionResult GetRoles([FromHeader] string authorization);

        [HttpPost("category")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveCategoryAsync([FromHeader] string authorization, [FromBody] Category category);

        [HttpDelete("category")]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteCategoryAsync([FromHeader] string authorization, [FromBody] Category category);

        [HttpGet("information")]
        [Authorize(Roles = Restrictions.AuthorizedRoles)]
        public abstract IActionResult GetInformationAsync([FromHeader] string authorization);

        //TODO: replace [FromForm] to [FromBody], change rile handling
        [HttpPost("project"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> SaveProjectAsync([FromHeader] string authorization, [FromForm] Project project);

        [HttpDelete("project"), DisableRequestSizeLimit]
        [Authorize(Roles = Restrictions.EditorRoles)]
        public abstract Task<IActionResult> DeleteProjectAsync([FromHeader] string authorization, [FromBody] Project project);

    }
}
