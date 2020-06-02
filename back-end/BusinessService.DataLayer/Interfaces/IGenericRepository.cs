using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessService.DataLayer.Interfaces
{
    //TODO: maybe I should discard idea of a generic repository?
    public interface IGenericRepository
    {
        Task<int> CountAsync<T>() where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        Task<T> FirstOrDefaultAsync<T>(params Expression<Func<T, object>>[] includes) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        T FirstOrDefault<T>(params Expression<Func<T, object>>[] includes) where T : class;
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        Task<List<T>> GetAsync<T>(params Expression<Func<T, object>>[] includes) where T : class;

        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        Task<List<T>> GetAsync<T>(int start, int length, params Expression<Func<T, object>>[] includes) where T : class;
        Task<List<T>> GetAsync<T>(int start, int length, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
    }
}
