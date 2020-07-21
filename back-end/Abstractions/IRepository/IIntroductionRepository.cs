using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    interface IIntroductionRepository
    {
        Task<Introduction> GetIntroduction();
        Task<Introduction> CreateIntroduction(Introduction item);
        Task<Introduction> UpdateIntroduction(Introduction item);
        Task<bool> DeleteIntroduction(Introduction item);
    }
}
