using Abstractions.Model;
using System.Threading.Tasks;

namespace Abstractions.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategories();

        bool IsEverything(string code);
    }
}
