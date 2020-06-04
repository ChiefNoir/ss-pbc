using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary>News repository</summary>
    public interface INewsRepository
    {
        /// <summary>Get all news</summary>
        /// <returns>All news</returns>
        Task<News[]> GetNews();
    }
}
