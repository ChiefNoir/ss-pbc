using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IHashManager _hashManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(DataContext context, IConfiguration configuration, IHashManager hashManager)
        {
            _context = context;
            _hashManager = hashManager;
            _configuration = configuration;

            InitDefaults(); // TODO: doesn't look good.
        }

        /// <summary> Initialize default user </summary>
        private async void InitDefaults()
        {
            if (!_context.HasAccounts)
            {
                try
                {
                    var login = _configuration.GetSection("Default")?.GetSection("Admin")?.GetValue<string>("Login");
                    var pass = _configuration.GetSection("Default")?.GetSection("Admin")?.GetValue<string>("Password");

                    await Add(login, pass, RoleNames.Admin);
                    _context.HasAccounts = true;
                }
                catch
                {
                    //TODO: log
                }
            }
        }

        /// <summary> Add new account </summary>
        /// <param name="login">Login as plain text</param>
        /// <param name="plainTextPassword">Password as a plain text</param>
        /// <param name="role">Account role <see cref="RoleNames"/></param>
        /// <returns> </returns>
        public Task<int> Add(string login, string plainTextPassword, string role)
        {
            var hashedPassword = _hashManager.Hash(plainTextPassword);

            var user = new DataModel.Account
            {
                Login = login,
                Password = hashedPassword.HexHash,
                Salt = hashedPassword.HexSalt,
                Role = role
            };

            _context.Accounts.AddAsync(user);
            return _context.SaveChangesAsync(); // TODO: change int to something else
        }

        /// <summary> Get account </summary>
        /// <param name="login">Login as plain text</param>
        /// <param name="password">Password as plain text</param>
        /// <returns> <see cref="Account"/> or <code>null</code> if account is not found </returns>
        public async Task<Account> Get(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Login == login);
            if (account == null)
                return null;

            var hashedPassword = _hashManager.Hash(password, account.Salt);
            if (hashedPassword.HexHash != account.Password)
                return null;

            return new Account
            {
                Login = account.Login,
                Role = account.Role
            };
        }

    }
}
