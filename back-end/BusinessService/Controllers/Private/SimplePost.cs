using Abstractions.IRepository;
using Abstractions.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class SimplePost : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public SimplePost(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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

    }
}
