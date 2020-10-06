using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class PublicPingController : ControllerBase
    {
        private readonly ISupervisor _supervisor;

        public PublicPingController(ISupervisor supervisor)
        {
            _supervisor = supervisor;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var result = _supervisor.SafeExecute
            (
                () => { return "pong"; } 
            );

            return new JsonResult(result);
        }

    }
}
