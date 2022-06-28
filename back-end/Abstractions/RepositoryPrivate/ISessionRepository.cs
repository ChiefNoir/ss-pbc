using Abstractions.Models;

namespace Abstractions.RepositoryPrivate
{
    public interface ISessionRepository
    {
        Task SaveSessionAsync(Account account, string token, string fingerprint);
        Task CheckSessionAsync(string token, string fingerprint);
        Task FlushAsync();
    }
}
