using Abstractions.Models;

namespace Abstractions.ICache
{
    public interface IDataCache
    {
        /* IDataCache must be used only for public-end.
         * Anonymous users can get outdated data, but that is sacrifice I'm willing to make.
         * 
         * 
         * 
         */

        Task<Introduction?> GetIntroductionAsync();
        Task<IEnumerable<Category>?> GetCategoriesAsync();
        Task<IEnumerable<ProjectPreview>?> GetProjectsPreviewAsync();

        Task<bool> SaveAsync(Introduction item);
        Task<bool> SaveAsync(IEnumerable<Category> items);
        Task<bool> SaveAsync(IEnumerable<ProjectPreview> items);

        Task FlushAsync();
    }
}
