using Abstractions.Models;

namespace Abstractions.RepositoryPrivate
{
    public interface IPrivateProjectRepository
    {
        /// <summary> Delete existing <seealso cref="Project"/> </summary>
        /// <param name="project"> <seealso cref="Project"/> to delete </param>
        /// <returns> <c>true</c> if delete was successful </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<bool> DeleteAsync(Project project);

        /// <summary> Create or update <seealso cref="Project"/> </summary>
        /// <param name="project">New <seealso cref="Project"/> </param>
        /// <returns> <seealso cref="Project"/> stored in database </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Project> SaveAsync(Project project);
    }
}
