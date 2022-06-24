using Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.ICache
{
    public interface IDataCache
    {
        Task<Introduction?> GetIntroductionAsync();

        Task<bool> SaveAsync(Introduction item);
    }
}
