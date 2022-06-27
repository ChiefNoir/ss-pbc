using Abstractions.Exceptions;
using Abstractions.IRepositories;
using Abstractions.Models;
using Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task FlushAsync()
        {
            _context.Sessions.RemoveRange(_context.Sessions);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSessionAsync(Account account, string token, string fingerprint)
        {
            var oldSessions = _context.Sessions.Where(x => x.AccountId == account.Id);
            _context.Sessions.RemoveRange(oldSessions);

            var session = new Models.Session()
            {
                AccountId = account.Id!.Value,
                Token = token,
                Fingerprint = fingerprint,
                Id = Guid.NewGuid()
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task CheckSessionAsync(string token, string fingerprint)
        {
            var session = await _context.Sessions.AnyAsync(x => x.Token == token && x.Fingerprint == fingerprint);

            if (!session)
                throw new FraudException(TextMessages.SuspiciousBehavior);
        }

    }
}
