using Abstractions.Models;
using System.Threading.Tasks;

namespace Abstractions.IRepositories
{
    /// <summary> <see cref="Introduction"/> repository </summary>
    public interface IIntroductionRepository
    {
        /// <summary> Get <seealso cref="Introduction"/> from storage </summary>
        /// <returns> <seealso cref="Introduction"/> from storage </returns>
        Task<Introduction> GetAsync();

        /// <summary> Update <seealso cref="Introduction"/>  </summary>
        /// <param name="item"> Updated <seealso cref="Introduction"/>  </param>
        /// <returns> Updated <seealso cref="Introduction"/>  </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Introduction> SaveAsync(Introduction item);
    }
}
