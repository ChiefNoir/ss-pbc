using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class SimplePost : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;


        public SimplePost(ICategoryRepository categoryRepository, IProjectRepository projectRepository, IConfiguration configuration, IFileRepository fileRepository, IAccountRepository accountRepository)
        {
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
            _fileRepository = fileRepository;
            _accountRepository = accountRepository;
            _configuration = configuration;
        }


        [HttpPost("category")]
        public async Task<IActionResult> Save([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.SaveCategory(category);
            });

            return new JsonResult(result);
        }

        [HttpDelete("category")]
        public async Task<IActionResult> Delete([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.DeleteCategory(category);
            });

            return new JsonResult(result);
        }

        [HttpPost("project")]
        public async Task<IActionResult> Save([FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Save(project);
            });

            return new JsonResult(result);
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var result = await Supervisor.SafeExecuteAsync(async () =>
            {
                var filename = await _fileRepository.Save(Request.Form.Files.FirstOrDefault());

                return FormatToUrl(filename);
            });

            return new JsonResult(result);
        }

        private string FormatToUrl(string name)
        {
            return 
            _configuration.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
            + "/" + 
            _configuration.GetSection("Location:FileStorage").Get<string>()
            + "/"
            + name;
        }


        [HttpGet("accounts")]
        public async Task<IActionResult> GetUsers([FromQuery] Paging paging, [FromQuery] AccountSearch searchQuery)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Search(paging.Start, paging.Length, searchQuery.Keyword);             
            });

            return new JsonResult(result);
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> AddUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Add(account);
            });

            return new JsonResult(result);
        }

        [HttpPatch("accounts")]
        public async Task<IActionResult> UpdateUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Update(account);
            });

            return new JsonResult(result);
        }

        [HttpDelete("accounts")]
        public async Task<IActionResult> DeleteUser([FromBody] Account account)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _accountRepository.Remove(account);
            });

            return new JsonResult(result);
        }

    }
}
