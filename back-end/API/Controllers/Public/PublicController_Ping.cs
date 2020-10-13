using Abstractions.API;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public
{
    public partial class PublicController : PublicControllerBase
    {
        public override IActionResult Ping()
        {
            var result = _supervisor.SafeExecute
            (
                () => { return "pong"; }
            );

            return new JsonResult(result);
        }
    }
}
