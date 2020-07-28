using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary>Projects repository</summary>
    public interface IProjectRepository
    {
        /// <summary> Create new <seealso cref="Project"/> </summary>
        /// <param name="project">New <seealso cref="Project"/> </param>
        /// <returns> <seealso cref="Project"/> stored in database </returns>
        Task<Project> Create(Project project);

        /// <summary> Delete existing <seealso cref="Project"/> </summary>
        /// <param name="project"> <seealso cref="Project"/> to delete </param>
        /// <returns> Number of rows affected in db </returns>
        Task<int> Delete(Project project);

        /// <summary> Get <seealso cref="ProjectPreview"/> (with paging)</summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <param name="categoryCode">Category code</param>
        /// <returns><seealso cref="ProjectPreview"/></returns>
        Task<ProjectPreview[]> GetProjectsPreview(int start, int length, string categoryCode);

        /// <summary> Get <seealso cref="Project"/> by code </summary>
        /// <param name="code"> <seealso cref="Project"/> code </param>
        /// <returns> <seealso cref="Project"/> </returns>
        Task<Project> Read(string code);

        /// <summary> Get <seealso cref="Project"/> by id </summary>
        /// <param name="id">Project id </param>
        /// <returns> <seealso cref="Project"/> </returns>
        Task<Project> Read(int id);

        /// <summary> Update existing <seealso cref="Project"/> </summary>
        /// <param name="project"> <seealso cref="Project"/> </param>
        /// <returns> Updated <seealso cref="Project"/> </returns>
        Task<Project> Update(Project project);
    }
}