using Abstractions.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface IProjectRepository
    {
        Task<ProjectPreview[]> GetProjects(int start, int length, string categoryCode);

        Task<Project> GetProject(string code);

        Task<int> Count();

        Task<int> Count(string categoryCode);
    }
}
