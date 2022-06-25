using Abstractions.Models;

namespace Abstractions.IRepositories
{
    /// <summary> Category repository</summary>
    public interface ICategoryRepository
    {
        /// <summary> Delete <seealso cref="Category"/>  from storage </summary>
        /// <param name="category"><seealso cref="Category"/> to delete</param>
        /// <returns> <c>true</c> if deleting was successful </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<bool> DeleteAsync(Category category);

        /// <summary>Get all categories from storage</summary>
        /// <returns>All categories in the storage</returns>
        Task<IEnumerable<Category>> GetAsync();

        /// <summary> Get <seealso cref="Category"/> by id </summary>
        /// <param name="id">Category id</param>
        /// <returns><seealso cref="Category"/>  or <b>null</b> </returns>
        Task<Category> GetAsync(Guid? id);

        /// <summary> Get <seealso cref="Category"/> by code </summary>
        /// <param name="code">Category code</param>
        /// <returns><seealso cref="Category"/>  or <b>null</b> </returns>
        Task<Category> GetAsync(string code);

        /// <summary>Save item to the storage </summary>
        /// <param name="item">New or updated <seealso cref="Category"/> </param>
        /// <returns>Created or updated <seealso cref="Category"/> </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Category> SaveAsync(Category item);
    }
}
