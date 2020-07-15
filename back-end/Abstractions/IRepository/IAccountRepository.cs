using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Account repository </summary>
    public interface IAccountRepository
    {
        /// <summary> Get existing <see cref="Account"/> </summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> Get(string login, string plainTextPassword);

        /// <summary>  Get existing <see cref="Account"/> </summary>
        /// <param name="id">Account id</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> Get(int id);

        Task<Account> Add(Account account);

        /// <summary> Update existing <see cref="Account"/> </summary>
        /// <param name="account"> Edited <see cref="Account"/></param>
        /// <returns></returns>
        Task<Account> Update(Account account);

        Task<bool> Remove(Account account);

        /// <summary> Search within <see cref="Account"/>, with paging </summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <returns> Array of <see cref="Account"/>, containing keyword </returns>
        Task<Account[]> Search(int start, int length);

        /// <summary> Count <see cref="Account"/></summary>
        /// <returns>Total of the accounts, containing keyword </returns>
        Task<int> Count();
    }
}
