using Abstractions.API;
using Abstractions.Model.System;
using Microsoft.AspNetCore.Mvc;
using Security.Extensions;
using System.Linq;
using System.Reflection;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override IActionResult GetInformationAsync()
        {
            var result = _supervisor.SafeExecute(() =>
            {
                var token = Request.Headers["Authorization"].ToString();
                var claims = _tokenManager.ValidateToken(token.Split(' ').Last());

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