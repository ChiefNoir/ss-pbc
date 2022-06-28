using Abstractions.Models;

namespace Abstractions.RepositoryPrivate
{
    public interface IPrivateCategoryRepository
    {
        /// <summary> Delete <seealso cref="Category"/>  from storage </summary>
        /// <param name="category"><seealso cref="Category"/> to delete</param>
        /// <returns> <c>true</c> if deleting was successful </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<bool> DeleteAsync(Category category);

        /// <summary>Save item to the storage </summary>
        /// <param name="item">New or updated <seealso cref="Category"/> </param>
        /// <returns>Created or updated <seealso cref="Category"/> </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Category> SaveAsync(Category item);
    }
}
