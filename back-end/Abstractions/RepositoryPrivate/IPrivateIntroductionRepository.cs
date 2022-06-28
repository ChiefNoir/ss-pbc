using Abstractions.Models;

namespace Abstractions.RepositoryPrivate
{
    public interface IPrivateIntroductionRepository
    {
        /// <summary> Update <seealso cref="Introduction"/>  </summary>
        /// <param name="item"> Updated <seealso cref="Introduction"/>  </param>
        /// <returns> Updated <seealso cref="Introduction"/>  </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Introduction> SaveAsync(Introduction item);
    }
}
