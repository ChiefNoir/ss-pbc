using Abstractions.IRepository;
using Infrastructure.DataModel;
using Newtonsoft.Json;
using System;

namespace Infrastructure.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly DataContext _context;

        public LogRepository(DataContext context)
        {
            _context = context;
        }

        public void LogError(Exception exception)
        {
            var item = new Log
            {
                DateTime = DateTime.Now,
                Source = exception.GetType().Name, //mostly to differ regular exception from custom
                Details = JsonConvert.SerializeObject(exception, Formatting.Indented)
            };

            _context.Log.Add(item);
            _context.SaveChanges();;
        }

    }
}
