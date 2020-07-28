using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Infrastructure.Repository;
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
    public class PrivateProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public PrivateProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost("project")]
        public async Task<IActionResult> Save([FromBody] Project project)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _projectRepository.Create(project);
            });

            return new JsonResult(result);
        }

    }
}
