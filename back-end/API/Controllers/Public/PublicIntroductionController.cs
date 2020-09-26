using Abstractions.IRepository;
using Abstractions.Supervision;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    [ApiController]
    [Route("api/v1/")]
    public class PublicIntroductionController : ControllerBase
    {
        private readonly IIntroductionRepository _introductionRepository;
        private readonly ISupervisor _supervisor;

        public PublicIntroductionController(IIntroductionRepository introductionRepository, ISupervisor supervisor)
        {
            _introductionRepository = introductionRepository;
            _supervisor = supervisor;
        }

        [HttpGet("introduction")]
        public async Task<IActionResult> GetIntroduction()
        {
            var result = await _supervisor.SafeExecuteAsync(() =>
            {
                return _introductionRepository.GetAsync();
            });

            return new JsonResult(result);
        }
    }
}