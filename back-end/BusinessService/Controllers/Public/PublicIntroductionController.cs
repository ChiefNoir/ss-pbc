using Abstractions.IRepository;
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

        public PublicIntroductionController(IIntroductionRepository introductionRepository)
        {
            _introductionRepository = introductionRepository;
        }

        [HttpGet("introduction")]
        public async Task<IActionResult> GetIntroduction()
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _introductionRepository.GetAsync();
            });

            return new JsonResult(result);
        }

    }
}
