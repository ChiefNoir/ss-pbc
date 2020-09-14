using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Model;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHashManager _hashManager;

        public AccountRepository(DataContext context, IConfiguration configuration, IHashManager hashManager)
        {
            _context = context;
            _configuration = configuration;
            _hashManager = hashManager;
        }



        public Task<int> CountAsync()
        {
            return _context.Accounts.CountAsync();
        }


        public async Task<bool> DeleteAsync(Account item)
        {
            if (item.Id == null)
                throw new Exception($"Can't delete new account {item.Login}");

            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (item.Id == null)
                throw new Exception($"Can't find account {item.Login}");

            _context.Accounts.Remove(acc);
            var rows = await _context.SaveChangesAsync();

            return rows == 1;
        }


        public async Task<Account> GetAsync(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new Exception($"Can't find account with id: {id}");

            return DataConverter.ToAccount(account);
        }

        public async Task<Account> GetAsync(string login, string password)
        {
            // This is the primary method for user verification
            // so, if there is no users in the db, we must create new one:
            if(await _context.Accounts.AnyAsync())
            {
                var newLogin = _configuration.GetSection("Default:Admin:Login").Get<string>();
                var newPass = _configuration.GetSection("Default:Admin:Password").Get<string>();
                
                await SaveAsync(new Account { Login = newLogin, Password = newPass, Role = RoleNames.Admin });
            }


            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw new Exception($"Login or password is empty");

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Login == login);
            if (account == null)
                throw new Exception($"Can't delete new account {login}");

            var hashedPassword = _hashManager.Hash(password, account.Salt);
            if (hashedPassword.HexHash != account.Password)
                throw new Exception($"Password mismatch");

            return new Account
            {
                Login = account.Login,
                Role = account.Role
            };
        }


        public Task<Account> SaveAsync(Account item)
        {
            if (item.Id == null)
                return CreateAsync(item);

            return UpdateAsync(item);
        }
        

        public Task<Account[]> SearchAsync(int start, int length)
        {
            return _context.Accounts
                           .Skip(start)
                           .Take(length)
                           .Select(x => DataConverter.ToAccount(x))
                           .ToArrayAsync();
        }



        private async Task<Account> CreateAsync(Account item)
        {
            if (string.IsNullOrEmpty(item.Login))
                throw new Exception("Login can't be empty");

            if (string.IsNullOrEmpty(item.Password))
                throw new Exception("Password can't be empty");

            var hashedPassword = _hashManager.Hash(item.Password);

            var account = AbstractionsConverter.ToAccount(item, hashedPassword);

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return DataConverter.ToAccount(account);
        }
        
        private async Task<Account> UpdateAsync(Account item)
        {
            if (string.IsNullOrEmpty(item.Login))
                throw new Exception("Login can't be empty");

            var dbAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == item.Id);
            if (dbAccount == null)
                throw new Exception($"Can't find account with id: {item.Id}");

            dbAccount.Login = item.Login;
            dbAccount.Role = item.Role;
            dbAccount.Version++;

            if (!string.IsNullOrEmpty(item.Password))
            {
                var hashedPassword = _hashManager.Hash(item.Password);

                dbAccount.Password = hashedPassword.HexHash;
                dbAccount.Salt = hashedPassword.HexSalt;
            }

            await _context.SaveChangesAsync();

            return DataConverter.ToAccount(dbAccount);
        }
    }
}