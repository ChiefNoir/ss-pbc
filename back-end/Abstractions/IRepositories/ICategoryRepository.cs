using Abstractions.Models;

namespace Abstractions.IRepositories
{
    /// <summary> Category repository</summary>
    public interface ICategoryRepository
    {
        /// <summary> Get <seealso cref="Category"/> by id </summary>
        /// <param name="id">Category id</param>
        /// <returns><seealso cref="Category"/>  or <b>Exception</b> </returns>
        Task<Category> GetAsync(Guid? id);

        /// <summary> Get <seealso cref="Category"/> by code </summary>
        /// <param name="code">Category code</param>
        /// <returns><seealso cref="Category"/>  or <seealso cref="Exception"/> </returns>
        Task<Category> GetAsync(string code);

        /// <summary>Get all categories from storage</summary>
        /// <returns>All categories in the storage</returns>
        Task<IEnumerable<Category>> GetAsync();
    }
}
