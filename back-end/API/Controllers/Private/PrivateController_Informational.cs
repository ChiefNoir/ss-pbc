using Abstractions.Model;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using Security.Extensions;
using System.Reflection;
using Abstractions.ISecurity;
using Abstractions.Model.System;
using Abstractions.API;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override IActionResult GetInformationAsync([FromHeader] string token)
        {
            var result = _supervisor.SafeExecute(token, new[] { RoleNames.Admin, RoleNames.Demo }, () =>
            {
                var claims = _tokenManager.ValidateToken(token);

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