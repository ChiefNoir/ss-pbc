using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateIntroductionController
    {
        private readonly IIntroductionRepository _introductionRepository;

        public PrivateIntroductionController(IIntroductionRepository introductionRepository)
        {
            _introductionRepository = introductionRepository;
        }

        [HttpPatch("introduction")]
        public async Task<IActionResult> UpdateIntroduction([FromBody] Introduction introduction)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _introductionRepository.UpdateIntroduction(introduction);
            });

            return new JsonResult(result);
        }
    }
}