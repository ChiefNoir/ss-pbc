using Abstractions.Model;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using Security.Extensions;
using System.Reflection;
using Abstractions.ISecurity;
using Abstractions.Model.System;
using Abstractions.API;
using System.Linq;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override IActionResult GetInformationAsync([FromHeader] string authorization)
        {
            var result = _supervisor.SafeExecute(
                () =>
            {
                var cleanToke = authorization.Split(' ').Last();

                var claims = _tokenManager.ValidateToken(cleanToke);

                return new Information
                {
                    Login = claims.Identity.Name,
                    Role = string.Join(';', claims.GetRoles()),
                    ApiVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
            });

            return new JsonResult(result);
        }
    }
}