using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface IIntroductionRepository
    {
        Task<Introduction> GetIntroduction();
        Task<Introduction> CreateIntroduction(Introduction item);
        Task<Introduction> UpdateIntroduction(Introduction item);
        Task<int> DeleteIntroduction(Introduction item);
    }
}
