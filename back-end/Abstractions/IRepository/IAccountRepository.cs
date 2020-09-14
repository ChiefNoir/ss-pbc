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
        /// <param name="item"><see cref="Account"/> to delete</param>
        /// <returns> <c>true</c> if deleting was successful </returns>
        Task<bool> DeleteAsync(Account item);

        /// <summary> Get existing <see cref="Account"/> </summary>
        /// <param name="id">Account id</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> GetAsync(int id);

        /// <summary> Get existing <see cref="Account"/> </summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> GetAsync(string login, string plainTextPassword);

        /// <summary>Save <see cref="Account"/> to the storage </summary>
        /// <param name="item">New or updated account</param>
        /// <returns>Created or updated <see cref="Account"/> </returns>
        Task<Account> SaveAsync(Account item);

        /// <summary> Search within <see cref="Account"/>, with paging </summary>
        /// <param name="start">Start</param>
        /// <param name="length">Length</param>
        /// <returns> Array of <see cref="Account"/> </returns>
        Task<Account[]> SearchAsync(int start, int length);
    }
}