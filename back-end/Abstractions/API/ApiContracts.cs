using Abstractions.Model;
using Abstractions.Model.Queries;
using Abstractions.Model.System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Abstractions.API
{
    [ApiController]
    [Route("api/v1/")]
    public abstract class PublicControllerBase : ControllerBase
    {
        [HttpGet("ping")]
        public abstract IActionResult Ping();
    
        [HttpGet("categories")]
        public abstract Task<IActionResult> GetCategoriesAsync();

        [HttpGet("categories/{id}")]
        public abstract Task<IActionResult> GetCategoryAsync(int id);

        [HttpGet("categories/everything")]
        public abstract Task<IActionResult> GetCategoryEverythingAsync();

        [HttpGet("introduction")]
        public abstract Task<IActionResult> GetIntroductionAsync();

        [HttpGet("projects/{code}")]
        public abstract Task<IActionResult> GetProjectAsync(string code);

        [HttpGet("projects/search")]
        public abstract Task<IActionResult> GetProjectsPreviewAsync([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery);
    }

    [ApiController]
    [Route("api/v1/")]
    public abstract class GatewayControllerBase : ControllerBase
    {
        [HttpPost("login")]
        public abstract Task<IActionResult> LoginAsync([FromBody] Credentials credentials);

        [HttpPost("token")]
        public abstract Task<IActionResult> ValidateAsync([FromHeader] string token);
    }


    [ApiController]
    [Route("api/v1/")]
    public abstract class PrivateControllerBase : ControllerBase
    {
        //TODO: replace [FromForm] to [FromBody], change rile handling
        [HttpPost("introduction"), DisableRequestSizeLimit]
        public abstract Task<IActionResult> SaveIntroductionAsync([FromHeader] string token, [FromForm] Introduction introduction);

        [HttpPost("accounts")]
        public abstract Task<IActionResult> SaveAccountAsync([FromHeader] string token, [FromBody] Account account);

        [HttpGet("accounts")]
        public abstract Task<IActionResult> CountAccountAsync([FromHeader] string token);

        [HttpDelete("accounts")]
        public abstract Task<IActionResult> DeleteAccountAsync([FromHeader] string token, [FromBody] Account account);

        [HttpGet("accounts/{id}")]
        public abstract Task<IActionResult> GetAccountAsync([FromHeader] string token, int id);

        [HttpGet("accounts/search")]
        public abstract Task<IActionResult> GetAccountsAsync([FromHeader] string token, [FromQuery] Paging paging);


        [HttpGet("roles")]
        public abstract IActionResult GetRoles([FromHeader] string token);

        [HttpPost("category")]
        public abstract Task<IActionResult> SaveCategoryAsync([FromHeader] string token, [FromBody] Category category);

        [HttpDelete("category")]
        public abstract Task<IActionResult> DeleteCategoryAsync([FromHeader] string token, [FromBody] Category category);

        [HttpGet("information")]
        public abstract IActionResult GetInformationAsync([FromHeader] string token);

        //TODO: replace [FromForm] to [FromBody], change rile handling
        [HttpPost("project"), DisableRequestSizeLimit]
        public abstract Task<IActionResult> SaveProjectAsync([FromHeader] string token, [FromForm] Project project);

        [HttpDelete("project"), DisableRequestSizeLimit]
        public abstract Task<IActionResult> DeleteProjectAsync([FromHeader] string token, [FromBody] Project project);

    }
}
