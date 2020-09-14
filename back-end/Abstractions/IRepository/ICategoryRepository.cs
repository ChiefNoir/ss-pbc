using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Category repository</summary>
    public interface ICategoryRepository
    {
        /// <summary> Count items in the storage</summary>
        /// <returns> Total of the items in the storage</returns>
        Task<int> CountAsync();

        /// <summary> Delete <see cref="Category"/>  from storage </summary>
        /// <param name="category"><see cref="Category"/> to delete</param>
        /// <returns> <c>true</c> if deleting was successful </returns>
        Task<bool> DeleteAsync(Category category);

        /// <summary>Get all categories from storage</summary>
        /// <returns>All categories in the storage</returns>
        Task<Category[]> GetAsync();

        /// <summary> Get <see cref="Category"/> by id </summary>
        /// <param name="id">Category id</param>
        /// <returns><see cref="Category"/>  or <b>null</b> </returns>
        Task<Category> GetAsync(int id);

        /// <summary> Get <see cref="Category"/> by code </summary>
        /// <param name="code">Category code</param>
        /// <returns><see cref="Category"/>  or <b>null</b> </returns>
        Task<Category> GetAsync(string code);

        /// <summary>Get technical <see cref="Category"/> for filtering by everything</summary>
        /// <returns>Everything <see cref="Category"/> </returns>
        Task<Category> GetTechnicalAsync();

        /// <summary>Save item to the storage </summary>
        /// <param name="item">New or updated <see cref="Category"/> </param>
        /// <returns>Created or updated <see cref="Category"/> </returns>
        Task<Category> SaveAsync(Category item);
    }
}