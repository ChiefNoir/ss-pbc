using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Account repository </summary>
    public interface IAccountRepository
    {
        /// <summary> Count items in the storage</summary>
        /// <returns> Total of the items in the storage</returns>
        Task<int> CountAsync();

        /// <summary> Delete item from storage </summary>
        /// <param name="item"><seealso cref="Account"/> to delete</param>
        /// <returns> <c>true</c> if delete was successful </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<bool> DeleteAsync(Account item);

        /// <summary> Get existing <seealso cref="Account"/> </summary>
        /// <param name="id">Account id</param>
        /// <returns>Existing <seealso cref="Account"/> or <b>null</b> </returns>
        Task<Account> GetAsync(int id);

        /// <summary> Get existing <seealso cref="Account"/> </summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <returns>Existing <seealso cref="Account"/> or <b>null</b> </returns>
        Task<Account> GetAsync(string login, string plainTextPassword);

        /// <summary>Save <seealso cref="Account"/> to the storage </summary>
        /// <param name="item">New or updated account</param>
        /// <returns>Created or updated <seealso cref="Account"/> </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Account> SaveAsync(Account item);

        /// <summary> Search within <seealso cref="Account"/>, with paging </summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <returns> Array of <seealso cref="Account"/> </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<Account[]> SearchAsync(int start, int length);
    }
}
