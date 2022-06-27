using Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.IRepositories
{
    public interface ISessionRepository
    {
        Task<bool> SaveSession(Account account, string token, string fingerprint);
        Task<bool> CheckSession(string token, string fingerprint);
    }
}
