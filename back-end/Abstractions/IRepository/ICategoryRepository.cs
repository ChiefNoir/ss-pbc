using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Category repository</summary>
    public interface ICategoryRepository
    {
        /// <summary>Get all categories</summary>
        /// <returns>All categories</returns>
        Task<Category[]> GetCategories();

        Task<Category[]> GetCategoriesOld();

        /// <summary>Check is category is technical for select without filter</summary>
        /// <param name="code">Category code</param>
        /// <returns><b>true</b> if category is technical for select without filter</returns>
        bool CheckIsEverything(string code);
    }
}
