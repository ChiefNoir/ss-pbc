using Abstractions.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface INewsRepository
    {
        Task<News[]> GetNews();
    }
}
