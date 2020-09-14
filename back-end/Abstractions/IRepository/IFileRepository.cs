using Microsoft.AspNetCore.Http;

namespace Abstractions.IRepository
{
    /// <summary> File repository</summary>
    public interface IFileRepository
    {
        /// <summary> Save <see cref="IFormFile"/> to local storage</summary>
        /// <param name="file"> File to save </param>
        /// <returns> Relative path to file </returns>
        string Save(IFormFile file);
    }
}
