using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
using Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class IntroductionRepository : IIntroductionRepository
    {
        private readonly DataContext _context;


        public IntroductionRepository(DataContext context)
        {
            _context = context;
        }


        public Task<Introduction> GetAsync()
        {
            return _context.Introductions
                           .Include(x => x.ExternalUrls)
                           .ThenInclude(x => x.ExternalUrl)
                           .AsNoTracking()
                           .Select(x => DataConverter.ToIntroduction(x))
                           .FirstOrDefaultAsync();
        }


        public async Task<Introduction> SaveAsync(Introduction item)
        {
            var dbItem = await _context.Introductions
                                       .Include(x => x.ExternalUrls)
                                       .ThenInclude(x => x.ExternalUrl)
                                       .FirstOrDefaultAsync();

            ModelValidation.Check(dbItem, item);

            Merge(dbItem, item);
            await _context.SaveChangesAsync();

            return DataConverter.ToIntroduction(dbItem);
        }


        private void Merge(DataModel.Introduction dbItem, Introduction newItem)
        {
            dbItem.Title = newItem.Title;
            dbItem.Content = newItem.Content;
            dbItem.PosterDescription = newItem.PosterDescription;
            dbItem.PosterUrl = newItem.PosterUrl;
            dbItem.Version++;


            Merge(dbItem, newItem.ExternalUrls);
        }

        private void Merge(DataModel.Introduction dbItem, IEnumerable<ExternalUrl> externalUrls)
        {
            foreach (var item in dbItem.ExternalUrls)
            {
                var localExternalUrl = externalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (localExternalUrl == null)
                {
                    _context.ExternalUrls.Remove(item.ExternalUrl);
                }
                else
                {
                    item.ExternalUrl.DisplayName = localExternalUrl.DisplayName;
                    item.ExternalUrl.Url = localExternalUrl.Url;
                    item.ExternalUrl.Version++;
                }
            }

            foreach (var item in externalUrls.Where(x => x.Id == null))
            {
                dbItem.ExternalUrls.Add(AbstractionsConverter.ToIntroductionExternalUrl(item));
            }
        }
    }
}