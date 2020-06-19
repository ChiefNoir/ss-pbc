using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Category repository</summary>
    public interface ICategoryRepository
    {
        /// <summary>Check is category is technical for select without filter</summary>
        /// <param name="code">Category code</param>
        /// <returns><b>true</b> if category is technical for select without filter</returns>
        Task<bool> CheckIsEverything(string code);

        /// <summary>Get all categories</summary>
        /// <returns>All categories</returns>
        Task<Category[]> GetCategories();

        /// <summary> Get <see cref="Category"/> by code </summary>
        /// <param name="code">Category code</param>
        /// <returns><see cref="Category"/>  or <b>null</b> </returns>
        Task<Category> GetCategory(string code);

        /// <summary>Get technical for filtering by everything</summary>
        /// <returns>Everything category</returns>
        Task<Category> GetEverythingCategory();

        Task<Category> SaveCategory(Category category);

        Task<Category> DeleteCategory(Category category);

    }
}