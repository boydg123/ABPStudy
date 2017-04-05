using System;
using System.Threading.Tasks;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// An interface to work with cache in a typed manner.Use <see cref="CacheExtensions.AsTyped{TKey,TValue}"/> method to convert a <see cref="ICache"/> to this interface.
    /// 以类型化方式与缓存一起工作的接口,用<see cref="CacheExtensions.AsTyped{TKey,TValue}"/>方法转换<see cref="ICache"/>至这个接口
    /// </summary>
    /// <typeparam name="TKey">Key type for cache items / 缓存项的键类型</typeparam>
    /// <typeparam name="TValue">Value type for cache items / 缓存项的值类型</typeparam>
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// Unique name of the cache.
        /// 缓存的唯一名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Default sliding expire time of cache items.
        /// 缓存项的默认滑动过期时间
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// Gets the internal cache.
        /// 获取内部缓存
        /// </summary>
        ICache InternalCache { get; }

        /// <summary>
        /// Gets an item from the cache.
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <param name="factory">Factory method to create cache item if not exists / 如果不存在使用工厂方法创建缓存项</param>
        /// <returns>Cached item / 缓存项</returns>
        TValue Get(TKey key, Func<TKey, TValue> factory);

        /// <summary>
        /// Gets an item from the cache.
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <param name="factory">Factory method to create cache item if not exists / 如果不存在使用工厂方法创建缓存项</param>
        /// <returns>Cached item / 缓存项</returns>
        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <returns>Cached item or null if not found / 缓存项目 / 没有找到则是null</returns>
        TValue GetOrDefault(TKey key);

        /// <summary>
        /// Gets an item from the cache or null if not found.
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <returns>Cached item or null if not found / 缓存项目 / 没有找到则是null</returns>
        Task<TValue> GetOrDefaultAsync(TKey key);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// 通过键保存/覆盖缓存中的项
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <param name="value">Value / 缓存的数据</param>
        /// <param name="slidingExpireTime">Sliding expire time / 滑动过期时间</param>
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// 通过键保存/覆盖缓存中的项
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        /// <param name="value">Value / 缓存的数据</param>
        /// <param name="slidingExpireTime">Sliding expire time / 滑动过期时间</param>
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// Removes a cache item by it's key (does nothing if given key does not exists in the cache).
        /// 移除指定键的缓存(如果指定的key不存在缓存中，则啥事也不做)
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        void Remove(TKey key);

        /// <summary>
        /// Removes a cache item by it's key.
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">Key / 缓存键</param>
        Task RemoveAsync(TKey key);

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