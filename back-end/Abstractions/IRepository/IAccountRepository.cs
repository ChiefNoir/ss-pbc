using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    /// <summary> Account repository </summary>
    public interface IAccountRepository
    {
        /// <summary> Get existing <see cref="Account"/> or <see cref="null"/> </summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <returns>Existing <see cref="Account"/> or <see cref="null"/> </returns>
        Task<Account> Get(string login, string plainTextPassword);

        /// <summary> Add new <see cref="Account"/></summary>
        /// <param name="login">Account login as plain text</param>
        /// <param name="plainTextPassword">Account password as plaint text</param>
        /// <param name="role">Account role</param>
        /// <returns>Numbers of states in the db affected in db</returns>
        Task<int> Add(string login, string plainTextPassword, string role);
    }
}
