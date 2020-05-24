using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessService.DataLayer.Interfaces
{
    //TODO: maybe I should discard idea of a generic repository?
    public interface IGenericRepository
    {
        int Count<T>() where T : class;
        int Count<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        
        T FirstOrDefault<T>(params Expression<Func<T, object>>[] includes) where T : class;
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        IEnumerable<T> Get<T>(params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<T> Get<T>(int start, int length, params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<T> Get<T>(int start, int length, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
    }
}
