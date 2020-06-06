﻿using System;
using System.Threading.Tasks;

namespace Abstractions.MemoryCache
{
    /// <summary> In-memory cache</summary>
    /// <typeparam name="TKey">Unique key type to identify item stored in cache</typeparam>
    /// <typeparam name="TEntity">Item to store in cache</typeparam>
    public interface IMemoryCache<TKey, TEntity>
    {
        /// <summary> Get all items int cache</summary>
        /// <param name="initAll">Function to initialize all items in cache</param>
        /// <returns>All items in cache</returns>
        Task<TEntity[]> GetAll(Func<Task<TEntity[]>> initAll);

        /// <summary> Get item stored in cache or add it</summary>
        /// <param name="key">Unique key to identify item stored in cache</param>
        /// <param name="createItem">Function to create item</param>
        /// <returns>Item stored in cache</returns>
        Task<TEntity> GetOrCreateAsync(TKey key, Func<Task<TEntity>> createItem);

        /// <summary> Remove item from cache</summary>
        /// <param name="key">Unique key to identify item stored in cache</param>
        void RemoveAsync(TKey key);

        /// <summary>Update or add item in/to cache</summary>
        /// <param name="key">Unique key to identify item stored in cache</param>
        /// <param name="createItem">Function to create item</param>
        void UpdateOrCreateAsync(TKey key, Func<Task<TEntity>> createItem);
    }
}