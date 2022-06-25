using Abstractions.Models;

namespace Abstractions.Cache
{
    /// <summary> Caching system </summary>
    public interface IDataCache
    {
        /// <summary> Get introduction from cache</summary>
        /// <returns> <seealso cref="Introduction"/> or <seealso cref="null"/></returns>
        Task<Introduction?> GetIntroductionAsync();

        /// <summary> Get all project preview from cache</summary>
        /// <returns> <seealso cref="ProjectPreview"/> or <seealso cref="null"/></returns>
        Task<IEnumerable<ProjectPreview>?> GetProjectPreviewAsync();

        /// <summary> Get IEnumerable<Category> from cache</summary>
        /// <returns> IEnumerable of <seealso cref="Category"/> or <seealso cref="null"/></returns>
        Task<IEnumerable<Category>?> GetCategoriesAsync();

        /// <summary> Save introduction to cache </summary>
        /// <param name="item">Introduction</param>
        /// <returns><code>true</code> if okay</returns>
        Task<bool> SaveAsync(Introduction item);

        /// <summary> Save categories to cache </summary>
        /// <param name="item">Categories</param>
        /// <returns><code>true</code> if okay</returns>
        Task<bool> SaveAsync(IEnumerable<Category> items);

        /// <summary> Save projects preview to cache</summary>
        /// <param name="item">ProjectPreview</param>
        /// <returns><code>true</code> if okay</returns>
        Task<bool> SaveAsync(IEnumerable<ProjectPreview> items);

        /// <summary> Flush <b>EVERYTHING</b> in the cache</summary>
        Task FlushAsync();

        /// <summary> Flush specific key from cache</summary>
        Task FlushAsync(CachedItemType itemType);
    }
}
