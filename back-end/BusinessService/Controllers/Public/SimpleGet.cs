using BusinessService.DataLayer.Interfaces;
using BusinessService.DataLayer.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers.Public
{
    [ApiController]
    [Route("api")]
    public class SimpleGet : ControllerBase
    {
        private readonly IGenericRepository _repository;

        public SimpleGet(IGenericRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("news")]
        public JsonResult GetNews()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                return _repository.Get<News>();
            });

            return new JsonResult(result);
        }

        [HttpGet("categories")]
        public JsonResult GetCategory()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                return _repository.Get<Category>();
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{start}/{length}/{categoryCode}")]
        public JsonResult GetProjects(int start, int length, string categoryCode)
        {
            var result = Supervisor.SafeExecute(() =>
            {
                var defaultCode = _repository.FirstOrDefault<Category>(x => x.IsEverything)?.Code;

                return _repository.Get<Project>
                (
                    start,
                    length,
                    f => defaultCode == categoryCode || string.IsNullOrEmpty(categoryCode) || f.Category.Code == categoryCode,
                    x => x.Category, x => x.ExternalUrls
                );
            });

            return new JsonResult(result);
        }

        [HttpGet("projects/{categoryCode}")]
        public JsonResult GetProjects(string categoryCode)
        {
            var result = Supervisor.SafeExecute(() =>
            {
                var defaultCode = _repository.FirstOrDefault<Category>(x => x.IsEverything)?.Code;

                return _repository.Count<Project>
                (
                    f => defaultCode == categoryCode || string.IsNullOrEmpty(categoryCode) || f.Category.Code == categoryCode
                );
            });

            return new JsonResult(result);
        }

        [HttpGet("projects")]
        public JsonResult GetProjects()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                return _repository.Count<Project>();
            });

            return new JsonResult(result);
        }

        [HttpGet("settings")]
        public JsonResult GetSettings()
        {
            var result = Supervisor.SafeExecute(() =>
            {
                return _repository.Get<ServerSetting>();
            });

            return new JsonResult(result);
        }

    }
}
