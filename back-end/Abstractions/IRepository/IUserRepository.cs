using Abstractions.Model;

namespace Abstractions.IRepository
{
    public interface IUserRepository
    {
        User GetUser(string login, string password);
    }
}
