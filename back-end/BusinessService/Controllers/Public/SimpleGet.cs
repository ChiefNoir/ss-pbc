using Abstractions.IRepository;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessService.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class SimpleGet : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly INewsRepository _newsRepository;

        public SimpleGet(ICategoryRepository categoryRepository, IProjectRepository projectRepository, INewsRepository newsRepository)
        {
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
            _newsRepository = newsRepository;
        }

        [HttpGet("news/all")]
        public async Task<IActionResult> GetNews()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _newsRepository.GetNews();
            });

            return new JsonResult(result);
        }

        private static readonly Dictionary<string, List<long>> _log = new Dictionary<string, List<long>>()
        {
            { "old", new List<long>()},
            { "new", new List<long>()},
        };

        [HttpGet("categories/all")]
        public async Task<IActionResult> GetCategories()
        {
            var st = Stopwatch.StartNew();
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategories();
            });

            st.Stop();
            _log["new"].Add(st.ElapsedMilliseconds);

            return new JsonResult(result);
        }

        [HttpGet("categories/all2")]
        public async Task<IActionResult> GetCategories2()
        {
            var st = Stopwatch.StartNew();
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategoriesOld();
            });

            st.Stop();
            _log["old"].Add(st.ElapsedMilliseconds);

            return new JsonResult(result);
        }

        [HttpGet("show")]
        public IActionResult Show()
        {
            var ss = $"old. Total:{_log["old"].Count}, Max:{_log["old"].Max()}, Min:{_log["old"].Min()}, Avg: {_log["old"].Average()}";

            ss += "\r\n";
            ss += $"new. Total:{_log["new"].Count}, Max:{_log["new"].Max()}, Min:{_log["new"].Min()}, Avg: {_log["new"].Average()}";

            return new JsonResult(ss);
        }


        [HttpGet("projects/count")]
        public async Task<IActionResult> GetProjectsTotal()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Count();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/count")]
        public async Task<IActionResult> GetProjectsTotal(string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Count(categoryCode);
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}/{start}/{length}/")]
        public async Task<IActionResult> GetProjects(int start, int length, string categoryCode)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProjects(start, length, categoryCode);

            });

            return new JsonResult(result);
        }

        [HttpGet("project/{code}")]
        public async Task<IActionResult> GetProject(string code)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProject(code);
            });

            return new JsonResult(result);
        }
    }
}
