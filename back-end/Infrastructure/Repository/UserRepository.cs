using Abstractions.IRepository;
using Abstractions.Model;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        List<User> users = new List<User>
        {
            new User { Login ="demo", Password ="demo", Role = "pidor"},
            new User { Login ="sa", Password ="sa", Role = RoleNames.Admin },
        };

        public User GetUser(string login, string password)
        {
            return users.FirstOrDefault(x => x.Login == login && x.Password == password);
        }
    }
}
