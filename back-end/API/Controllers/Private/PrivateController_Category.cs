using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        public override async Task<IActionResult> SaveCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.SaveAsync(category)
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> DeleteCategoryAsync([FromBody] Category category)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.DeleteAsync(category)
            );

            return new JsonResult(result);
        }
    }
}