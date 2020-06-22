using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary>Projects repository</summary>
    public interface IProjectRepository
    {
        /// <summary> Get <seealso cref="ProjectPreview"/> (with paging)</summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <param name="categoryCode">Category code</param>
        /// <returns><seealso cref="ProjectPreview"/></returns>
        Task<ProjectPreview[]> GetProjectsPreview(int start, int length, string categoryCode);

        /// <summary> Get <seealso cref="Project"/> (with paging)</summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <param name="categoryCode">Category code</param>
        /// <returns><seealso cref="Project"/></returns>
        Task<Project[]> GetProjects(int start, int length, string categoryCode);

        /// <summary> Get full specific <seealso cref="Project"/></summary>
        /// <param name="code">Project code</param>
        /// <returns>Full specific <seealso cref="Project"/></returns>
        Task<Project> GetProject(string code);

        /// <summary> Save <see cref="Project"/> </summary>
        /// <param name="project"><see cref="Project"/> to save </param>
        /// <returns>Saved <see cref="Project"/> </returns>
        Task<Project> Save(Project project);
    }
}
