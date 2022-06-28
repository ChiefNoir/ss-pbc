using Abstractions.Models;

namespace Abstractions.IRepositories
{
    /// <summary> <see cref="Introduction"/> repository </summary>
    public interface IIntroductionRepository
    {
        /// <summary> Get <seealso cref="Introduction"/> from storage </summary>
        /// <returns> <seealso cref="Introduction"/> from storage </returns>
        Task<Introduction> GetAsync();
    }
}
