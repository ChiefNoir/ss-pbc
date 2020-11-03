using Abstractions.Model.System;
using Abstractions.Supervision;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneralTests.SharedUtils
{
    static class ControllerContextCreator
    {
        public async static Task<ControllerContext> CreateValid(DataContext context)
        {
            var apiAuth = Storage.CreateGatewayController(context);
            var identity =
            (
                await apiAuth.LoginAsync
                (
                    new Credentials { Login = "sa", Password = "sa" }
                ) as JsonResult
            ).Value as ExecutionResult<Identity>;

            GenericChecks.CheckSucceed(identity);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = identity.Data.Token;
            
            return new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }

        public static ControllerContext CreateInvalid(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            return new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }


        public static async Task<ControllerContext> CreateValid(DataContext context, FormFileCollection files)
        {
            var apiAuth = Storage.CreateGatewayController(context);
            var identity =
            (
                await apiAuth.LoginAsync
                (
                    new Credentials { Login = "sa", Password = "sa" }
                ) as JsonResult
            ).Value as ExecutionResult<Identity>;

            GenericChecks.CheckSucceed(identity);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = identity.Data.Token;

            // the form itself isn't important
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), files);

            return new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }



    }
}
