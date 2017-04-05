using System;
using System.Threading.Tasks;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Defines a cache that can be store and get items by keys.
    /// 定义可以通过键存储和获取项的缓存
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Unique name of the cache.
        /// 缓存的唯一名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Default sliding expire time of cache items.Default value: 60 minutes (1 hour).Can be changed by configuration.
        /// 缓存项的默认滑动过期时间。默认值:60分钟(1个小时)。可以通过配置修改
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// Default absolute expire time of cache items.Default value: null (not used).
        /// 缓存项目的默认绝对过期时间。默认值:null(没有被使用)
        /// </summary>
        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        /// <summary>
        /// Gets an item from the cache.
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">Key / Key</param>
        /// <param name="factory">Factory method to create cache item if not exists / 如果不存在使用工厂方法创建缓存项</param>
        /// <returns>Cached item / 缓存项</returns>
        object Get(string key, Func<string, object> factory);

        /// <summary>
        /// Gets an item from the cache.
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">Key / Key</param>
        /// <param name="factory">Factory method to create cache item if not exists / 如果不存在使用工厂方法创建缓存项</param>
        /// <returns>Cached item / 缓存项</returns>
        Task<object> GetAsync(string key, Func<string, Task<object>> factory);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">Key / Key</param>
        /// <returns>Cached item or null if not found / 缓存项目 / 没有找到则是null</returns>
        object GetOrDefault(string key);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Cached item or null if not found / 缓存项目 / 没有找到则是null</returns>
        Task<object> GetOrDefaultAsync(string key);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// 通过键保存/覆盖缓存中的项
        /// Use one of the expire times at most (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>).
        /// If none of them is specified, then <see cref="DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="DefaultSlidingExpireTime"/> will be used.
        /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">Key / 缓存key</param>
        /// <param name="value">Value / 缓存的数据</param>
        /// <param name="slidingExpireTime">Sliding expire time / 滑动过期时间</param>
        /// <param name="absoluteExpireTime">Absolute expire time / 绝对过期时间</param>
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// 通过键保存/覆盖缓存中的项
        /// Use one of the expire times at most (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>).
        /// If none of them is specified, then <see cref="DefaultAbsoluteExpireTime"/> will be used if it's not null. Othewise, <see cref="DefaultSlidingExpireTime"/> will be used.
        /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">Key / 缓存key</param>
        /// <param name="value">Value / 缓存的数据</param>
        /// <param name="slidingExpireTime">Sliding expire time / 滑动过期时间</param>
        /// <param name="absoluteExpireTime">Absolute expire time / 绝对过期时间</param>
        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// Removes a cache item by it's key.
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">Key / 缓存key</param>
        void Remove(string key);

        /// <summary>
        /// Removes a cache item by it's key (does nothing if given key does not exists in the cache).
        /// 移除指定键的缓存(如果指定的key不存在缓存中，则啥事也不做)
        /// </summary>
        /// <param name="key">Key</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Clears all items in this cache.
        /// 清除所有的缓存数据
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears all items in this cache.
        /// 清除所有的缓存数据 - 异步
        /// </summary>
        Task ClearAsync();
    }
}