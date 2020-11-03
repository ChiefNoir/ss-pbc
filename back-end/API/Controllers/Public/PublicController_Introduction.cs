using Abstractions.API;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    public partial class PublicController : PublicControllerBase
    {
        public override async Task<IActionResult> GetIntroductionAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _introductionRepository.GetAsync()
            );

            return new JsonResult(result);
        }
    }
}