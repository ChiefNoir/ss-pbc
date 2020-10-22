using Abstractions.Model.System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Abstractions.API
{
    [ApiController]
    [Route("api/v1/")]
    public abstract class GatewayControllerBase : ControllerBase
    {
        [HttpPost("login")]
        public abstract Task<IActionResult> LoginAsync([FromBody] Credentials credentials);

        [HttpPost("token")]
        public abstract Task<IActionResult> ValidateAsync([FromHeader] string token);
    }
}
