using Abstractions.IRepository;
using Infrastructure.Converters;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Abstractions.Model.Introduction> CreateIntroduction(Abstractions.Model.Introduction item)
        {
            var hasDbItem = await _context.Introductions
                                          .AsNoTracking()
                                          .AnyAsync();

            if (hasDbItem)
                throw new Exception("Introduction is already created");

            var dbItem = AbstractionsConverter.ToIntroduction(item);
            _context.Introductions.Add(dbItem);

            await _context.SaveChangesAsync();

            return DataConverter.ToIntroduction(dbItem);
        }

        public async Task<int> DeleteIntroduction(Abstractions.Model.Introduction item)
        {
            var dbItem = await _context.Introductions.FirstOrDefaultAsync();
            if (dbItem == null)
                throw new Exception("Introduction is not found");

            _context.Remove(dbItem);

            return await _context.SaveChangesAsync();
        }

        public Task<Abstractions.Model.Introduction> GetIntroduction()
        {
            return _context.Introductions
                           .Include(x => x.ExternalUrls)
                           .ThenInclude(x => x.ExternalUrl)
                           .AsNoTracking()
                           .Select(x => DataConverter.ToIntroduction(x))                           
                           .FirstOrDefaultAsync();
        }

        public async Task<Abstractions.Model.Introduction> UpdateIntroduction(Abstractions.Model.Introduction item)
        {
            var dbItem = await _context.Introductions.FirstOrDefaultAsync();
            if (dbItem == null)
                throw new Exception("Introduction is not found");

            Merge(dbItem, item);
            await _context.SaveChangesAsync();

            return DataConverter.ToIntroduction(dbItem);
        }

        private void Merge(Introduction dbItem, Abstractions.Model.Introduction newItem)
        {
            dbItem.Content = newItem.Content;
            dbItem.PosterDescription = newItem.PosterDescription;
            dbItem.PosterUrl = newItem.PosterUrl;
            Merge(dbItem, newItem.ExternalUrls);
            dbItem.Version++;
        }

        private void Merge(Introduction dbItem, IEnumerable<Abstractions.Model.ExternalUrl> externalUrls)
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
