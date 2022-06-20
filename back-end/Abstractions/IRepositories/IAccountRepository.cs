using Abstractions.Models;

namespace Abstractions.IRepositories
{
    /// <summary> Account repository </summary>
    public interface IAccountRepository
    {
        /// <summary> Delete item from storage </summary>
        /// <param name="item"><seealso cref="Account"/> to delete</param>
        /// <returns> <c>true</c> if delete was successful </returns>
        /// <exception cref="Exceptions.InconsistencyException"/>
        Task<bool> DeleteAsync(Account item);

        /// <summary> Get existing <seealso cref="Account"/> </summary>
        /// <param name="id">Account id</param>
        /// <returns>Existing <seealso cref="Account"/> or <b>null</b> </returns>
        Task<Account> GetAsync(Guid id);

        /// <summary> Get existing <seealso cref="Account"/> </summary>
        /// <returns> Array of <seealso cref="Account"/> </returns>
        Task<Account[]> GetAsync();

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
    }
}
