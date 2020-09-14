using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IHashManager _hashManager;

        public AccountRepository(DataContext context, IConfiguration configuration, IHashManager hashManager)
        {
            _context = context;
            _hashManager = hashManager;
            _configuration = configuration;

            InitDefaults(); // TODO: doesn't look good.
        }

        public async Task<Account> AddAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.Login))
                throw new Exception("Login can't be empty");

            if (string.IsNullOrEmpty(account.Password))
                throw new Exception("Password can't be empty");

            var hashedPassword = _hashManager.Hash(account.Password);

            var user = new DataModel.Account
            {
                Login = account.Login,
                Password = hashedPassword.HexHash,
                Salt = hashedPassword.HexSalt,
                Role = account.Role
            };

            await _context.Accounts.AddAsync(user);
            await _context.SaveChangesAsync();

            return Convert(user);
        }

        public Task<int> CountAsync()
        {
            return _context.Accounts.CountAsync();
        }

        /// <summary> Get account </summary>
        /// <param name="login">Login as plain text</param>
        /// <param name="password">Password as plain text</param>
        /// <returns> <see cref="Account"/> or <code>null</code> if account is not found </returns>
        public async Task<Account> GetAsync(string login, string password)
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

        public async Task<Account> GetAsync(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                return null;

            return Convert(account);
        }

        public async Task<bool> DeleteAsync(Account account)
        {
            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);

            _context.Accounts.Remove(acc);
            var rows = await _context.SaveChangesAsync();
            
            return rows == 1;
        }

        public Task<Account[]> SearchAsync(int start, int length)
        {
            return _context.Accounts
                            .Skip(start)
                            .Take(length)
                            .Select(x => Convert(x))
                            .ToArrayAsync();
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);

            if (acc == null)
                throw new Exception("Not found");

            if(string.IsNullOrEmpty(account.Login))
                throw new Exception("Login can't be empty");

            acc.Login = account.Login;
            acc.Version++;
            acc.Role = account.Role;

            if (!string.IsNullOrEmpty(account.Password))
            {
                var hashedPassword = _hashManager.Hash(account.Password);

                acc.Password = hashedPassword.HexHash;
                acc.Salt = hashedPassword.HexSalt;
            }

            await _context.SaveChangesAsync();

            return Convert(acc);
        }

        private static Account Convert(DataModel.Account account)
        {
            return new Account
            {
                Id = account.Id,
                Login = account.Login,
                Role = account.Role,
                Password = null,
                Version = account.Version
            };
        }

        /// <summary> Initialize default user </summary>
        private async void InitDefaults()
        {
            if (!_context.HasAccounts)
            {
                try
                {
                    var login = _configuration.GetSection("Default:Admin:Login").Get<string>();
                    var pass = _configuration.GetSection("Default:Admin:Password").Get<string>();

                    await AddAsync(new Account { Login = login, Password = pass, Role = RoleNames.Admin });
                    _context.HasAccounts = true;
                }
                catch
                {
                    //TODO: log
                }
            }
        }
    }
}