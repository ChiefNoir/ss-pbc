using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GeneralTests.Common
{
    internal static class Utils
    {

        public static DataContext CreateContext<T>(T initialItem) where T : class
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("temp");

            var options = builder.Options;
            var context = new DataContext(options);
            context.Database.EnsureCreated();

            if (initialItem != null)
            {
                context.Set<T>().Add(initialItem);
                context.SaveChanges();
            }

            return context;
        }
    }
}
