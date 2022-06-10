using Abstractions.API;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Public
{
    public partial class PublicController : PublicControllerBase
    {
        public override async Task<IActionResult> GetCategoriesAsync()
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.GetAsync()
            );

            return new JsonResult(result);
        }

        public override async Task<IActionResult> GetCategoryAsync(int id)
        {
            var result = await _supervisor.SafeExecuteAsync
            (
                () => _categoryRepository.GetAsync(id)
            );

            return new JsonResult(result);
        }
    }
}