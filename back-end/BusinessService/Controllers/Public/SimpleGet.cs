using System;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers.Public
{
    public class SimpleGet : Controller
    {
        [HttpGet("news")]
        public JsonResult GetNews()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                throw new NotImplementedException();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories")]
        public JsonResult GetCategory()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                throw new NotImplementedException();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/all")]
        public JsonResult GetCategoryAll()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                throw new NotImplementedException();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{start}/{length}/{categoryCode}")]
        public JsonResult GetProjects(int start, int length, string categoryCode)
        {
            var result = Supervisor.SafeExecute(() =>
            {
                throw new NotImplementedException();
            });

            return new JsonResult(result);
        }
    }
}
