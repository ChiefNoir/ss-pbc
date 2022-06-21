﻿using Abstractions.Exceptions;
using Abstractions.IRepositories;
using Abstractions.Models;
using Abstractions.Security;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Security;
using System.Security;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly HashManager _hashManager;

        public AccountRepository(DataContext context, IConfiguration configuration, HashManager hashManager)
        {
            _context = context;
            _configuration = configuration;
            _hashManager = hashManager;
        }

        public async Task<bool> DeleteAsync(Account item)
        {
            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == item.Id);
            CheckBeforeDelete(acc, item);


            _context.Accounts.Remove(acc);
            var rows = await _context.SaveChangesAsync();

            return rows == 1;
        }

        public async Task<Account[]> GetAsync()
        {
            var result = await _context.Accounts
                                       .Select(x => DataConverter.ToAccount(x))
                                       .AsNoTracking()
                                       .ToArrayAsync();

            return result;
        }

        public async Task<Account> GetAsync(Guid id)
        {
            var result = await _context.Accounts
                                       .Where(x => x.Id == id)
                                       .Select(x => DataConverter.ToAccount(x))
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, "Account")
                    );
            }

            return result;
        }

        public async Task<Account> GetAsync(string login, string plainTextPassword)
        {
            // This is the primary method for user verification
            // so, if there is no users in the db, we must create new one:
            if (!await _context.Accounts.AnyAsync())
            {
                var newLogin = _configuration.GetSection("Default:Admin:Login").Get<string>();
                var newPass = _configuration.GetSection("Default:Admin:Password").Get<string>();

                await SaveAsync(new Account { Login = newLogin, Password = newPass, Role = RoleNames.Admin });
            }


            var dbItem = await _context.Accounts
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Login == login);

            CheckAccount(dbItem, login, plainTextPassword, _hashManager);

            return new Account
            {
                Id = dbItem.Id,
                Login = login,
                Role = dbItem.Role,
                Version = dbItem.Version
            };
        }


        public Task<Account> SaveAsync(Account item)
        {
            if (item.Id == null)
            {
                return CreateAsync(item);
            }

            return UpdateAsync(item);
        }

        private async Task<Account> CreateAsync(Account item)
        {
            CheckBeforeCreate(item);

            var hashedPassword = _hashManager.Hash(item.Password);

            var account = AbstractionsConverter.ToAccount(item, hashedPassword);

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return DataConverter.ToAccount(account);
        }

        private async Task<Account> UpdateAsync(Account item)
        {
            var dbAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == item.Id);
            CheckBeforeUpdate(dbAccount, item, _context);

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


        public void CheckBeforeCreate(Account account)
        {
            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Password))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Password")
                    );
            }

            if (string.IsNullOrEmpty(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Password")
                    );
            }

            var allRoles = RoleNames.GetRoles();

            if (!allRoles.Contains(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.TheRoleDoesNotExist, account.Role)
                    );
            }

            if (_context.Accounts.Any(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }

        private static void CheckBeforeDelete(Models.Account dbItem, Account account)
        {
            if (account.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, account.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if (dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }
        }

        private static void CheckBeforeUpdate(Models.Account dbItem, Account account, DataContext context)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Role")
                    );
            }

            var allRoles = RoleNames.GetRoles();

            if (!allRoles.Contains(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.TheRoleDoesNotExist, account.Role)
                    );
            }

            if (dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }

            if (dbItem.Login != account.Login && context.Accounts.Any(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }

        private static void CheckAccount(Models.Account dbItem, string login, string password, HashManager hashManager)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new SecurityException(Resources.TextMessages.AccountEmptyLogin);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new SecurityException(Resources.TextMessages.AccountEmptyPassword);
            }

            if (dbItem == null)
            {
                throw new SecurityException(Resources.TextMessages.AccountSomethingWrong);
            }

            var hashedPassword = hashManager.Hash(password, dbItem.Salt);
            if (hashedPassword?.HexHash != dbItem.Password)
            {
                throw new SecurityException(Resources.TextMessages.AccountSomethingWrong);
            }

        }
    }
}
