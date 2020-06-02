using BusinessService.DataLayer.Interfaces;
using BusinessService.DataLayer.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessService.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class SimpleGet : ControllerBase
    {
        private readonly IGenericRepository _repository;

        public SimpleGet(IGenericRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("news/all")]
        public async Task<IActionResult> GetNews()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _repository.GetAsync<News>();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories/all")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _repository.GetAsync<Category>();
            });

            return new JsonResult(result);
        }

        [HttpGet("settings/all")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _repository.GetAsync<ServerSetting>();
            });

            return new JsonResult(result);
        }


        [HttpGet("projects/count")]
        public async Task<IActionResult> GetProjectsTotal()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _repository.CountAsync<Project>();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/count")]
        public async Task<IActionResult> GetProjectsTotal(string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                var defaultCode = _repository.FirstOrDefault<Category>(x => x.IsEverything)?.Code;

                return _repository.CountAsync<Project>
                (
                    //TODO: fixit
                    f => defaultCode == categoryCode || string.IsNullOrEmpty(categoryCode) || f.Category.Code == categoryCode
                );
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/{start}/{length}/")]
        public async Task<IActionResult> GetProjects(int start, int length, string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                var defaultCode = _repository.FirstOrDefault<Category>(x => x.IsEverything)?.Code;

                return _repository.GetAsync<Project>
                (
                    start,
                    length,
                    f => defaultCode == categoryCode || string.IsNullOrEmpty(categoryCode) || f.Category.Code == categoryCode,
                    x => x.Category, x => x.ExternalUrls
                );
            });

            return new JsonResult(result);
        }

        [HttpGet("project/{code}")]
        public async Task<IActionResult> GetProject(string code)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _repository.FirstOrDefaultAsync<Project>(x => x.Code == code, x => x.Category, x => x.ExternalUrls);
            });

            return new JsonResult(result);
        }
    }
}
