using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface IIntroductionRepository
    {
        /// <summary> Get <seealso cref="Introduction"/> </summary>
        /// <returns> Introduction </returns>
        Task<Introduction> GetIntroduction();

        /// <summary> Update <seealso cref="Introduction"/> </summary>
        /// <param name="item"> Updated <seealso cref="Introduction"/> </param>
        /// <returns> Updated <seealso cref="Introduction"/> </returns>
        Task<Introduction> UpdateIntroduction(Introduction item);
    }
}