﻿using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("categories/all")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategories();
            });

            return new JsonResult(result);
        }

        [HttpGet("category/{code}")]
        public async Task<IActionResult> GetCategory(string code)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetCategory(code);
            });

            //TODO: error on null
            return new JsonResult(result);
        }



        [HttpGet("category/everything")]
        public async Task<IActionResult> GetEverythingCategory()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.GetEverythingCategory();
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

        [HttpGet("projects/search")]
        public async Task<IActionResult> GetProjectsPreview([FromQuery] Paging paging, [FromQuery] ProjectSearch searchQuery)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.GetProjectsPreview(paging.Start, paging.Length, searchQuery.CategoryCode);

            });

            return new JsonResult(result);
        }

    }
}
