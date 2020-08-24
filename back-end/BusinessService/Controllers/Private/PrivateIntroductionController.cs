using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateIntroductionController : ControllerBase
    {
        private readonly IIntroductionRepository _introductionRepository;

        public PrivateIntroductionController(IIntroductionRepository introductionRepository)
        {
            _introductionRepository = introductionRepository;
        }

        [HttpPatch("introduction")]
        public async Task<IActionResult> UpdateIntroduction([FromHeader] string token, [FromBody] Introduction introduction)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin }, () =>
            {
                return _introductionRepository.UpdateIntroduction(introduction);
            });

            return new JsonResult(result);
        }
    }
}