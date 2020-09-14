using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> <see cref="Introduction"/> repository </summary>
    public interface IIntroductionRepository
    {
        /// <summary> Get <see cref="Introduction"/> from storage </summary>
        /// <returns> <see cref="Introduction"/> from storage </returns>
        Task<Introduction> GetAsync();

        /// <summary> Update <see cref="Introduction"/>  </summary>
        /// <param name="item"> Updated <see cref="Introduction"/>  </param>
        /// <returns> Updated <see cref="Introduction"/>  </returns>
        Task<Introduction> SaveAsync(Introduction item);
    }
}