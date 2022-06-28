using Abstractions.Models;

namespace Abstractions.IRepositories
{
    /// <summary>Projects repository</summary>
    public interface IProjectRepository
    {
        /// <summary> Get <seealso cref="Project"/> by code </summary>
        /// <param name="code"> <seealso cref="Project"/> code </param>
        /// <returns> <seealso cref="Project"/> </returns>
        Task<Project> GetAsync(string code);

        /// <summary> Get <seealso cref="ProjectPreview"/> (with paging)</summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <param name="categoryCode">Category code</param>
        /// <returns><seealso cref="ProjectPreview"/></returns>
        Task<IEnumerable<ProjectPreview>> GetPreviewAsync(int start, int length, string categoryCode);
    }
}
