using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessService.DataLayer.Extensions
{
    public static class ExtIQueryable
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes == null || !includes.Any())
                return query;

            query = includes.Aggregate
            (
                query, (current, include) => current.Include(include)
            );

            return query;
        }
    }
}
