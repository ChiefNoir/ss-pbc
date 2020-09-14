using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Account repository </summary>
    public interface IAccountRepository
    {
        /// <summary>Add new <see cref="Account"/> to the storage </summary>
        /// <param name="account">New account</param>
        /// <returns>Created <see cref="Account"/> </returns>
        Task<Account> AddAsync(Account account);

        /// <summary> Count <see cref="Account"/> in the storage</summary>
        /// <returns> Total of the accounts </returns>
        Task<int> CountAsync();

        /// <summary> Delete <see cref="Account"/>  from storage </summary>
        /// <param name="account"><see cref="Account"/> to delete</param>
        /// <returns> <c>true</c> if deleting was successful </returns>
        Task<bool> DeleteAsync(Account account);

        /// <summary>  Get existing <see cref="Account"/> </summary>
        /// <param name="id">Account id</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> GetAsync(int id);

        /// <summary> Get existing <see cref="Account"/> </summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> GetAsync(string login, string plainTextPassword);

        
        /// <summary> Search within <see cref="Account"/>, with paging </summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <returns> Array of <see cref="Account"/>, containing keyword </returns>
        Task<Account[]> SearchAsync(int start, int length);

        /// <summary> Update existing <see cref="Account"/> </summary>
        /// <param name="account"> Edited <see cref="Account"/></param>
        /// <returns>Updated <see cref="Account"/> </returns>
        Task<Account> UpdateAsync(Account account);
    }
}