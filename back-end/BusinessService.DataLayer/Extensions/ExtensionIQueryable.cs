using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessService.DataLayer.Extensions
{
    /// <summary> Extension of the <seealso cref="IQueryable"/></summary>
    public static class ExtensionIQueryable
    {
        /// <summary> Include values from multiple foreign tables</summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <param name="query">Query</param>
        /// <param name="includes">Includes from foreign tables</param>
        /// <returns>Result of the query</returns>
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
