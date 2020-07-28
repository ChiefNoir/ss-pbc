using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface IFileRepository
    {
        /// <summary> Save <see cref="IFormFile"/></summary>
        /// <param name="file">File to save</param>
        /// <returns>Relative path to file</returns>
        Task<string> Save(IFormFile file);
    }
}