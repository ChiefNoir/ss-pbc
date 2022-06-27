using Abstractions.IRepositories;
using Abstractions.Models;

namespace Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveSession(Account account, string token, string fingerprint)
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

            return true;
        }

        public Task<bool> CheckSession(string token, string fingerprint)
        {
            return Task.FromResult(true);
        }
    }
}
