using Abstractions.Exceptions;
using Abstractions.IRepository;
using Abstractions.Model;
using Infrastructure.Converters;
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

            CheckBeforeUpdate(dbItem, item);

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

        private void Merge(DataModel.Introduction dbItem, IEnumerable<ExternalUrl> newExternalUrls)
        {
            if (newExternalUrls == null)
                newExternalUrls = new List<ExternalUrl>();

            var toRemove = dbItem.ExternalUrls.Where(eu => !newExternalUrls.Any(x => eu.ExternalUrlId == x.Id)).ToList();
            foreach (var item in toRemove)
            {
                dbItem.ExternalUrls.Remove(item);

                _context.ExternalUrls.Remove(item.ExternalUrl);
                _context.IntroductionExternalUrls.Remove(item);
            }

            var toAdd = newExternalUrls.Where(x => x.Id == null);
            foreach (var item in toAdd)
            {
                dbItem.ExternalUrls.Add(AbstractionsConverter.ToIntroductionExternalUrl(item));
            }

            var toUpdate = newExternalUrls.Where(x => x.Id.HasValue);
            foreach (var item in toUpdate)
            {
                var upd = dbItem.ExternalUrls.FirstOrDefault(x => x.ExternalUrlId == item.Id.Value);

                upd.ExternalUrl.DisplayName = item.DisplayName;
                upd.ExternalUrl.Url = item.Url;
                upd.ExternalUrl.Version++;
            }

        }


        private static void CheckBeforeUpdate(DataModel.Introduction dbItem, Introduction introduction)
        {

            if (dbItem.Version != introduction.Version)
            {
                throw new InconsistencyException
                (
                    string.Format(Resources.TextMessages.ItemWasAlreadyChanged, introduction.GetType().Name)
                );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = introduction.ExternalUrls?.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                    continue;

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }
        }
    }
}