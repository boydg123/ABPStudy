using System;
using System.Threading.Tasks;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Implements <see cref="ITypedCache{TKey,TValue}"/> to wrap a <see cref="ICache"/>.
    /// <see cref="ITypedCache{TKey,TValue}"/>的实现去包装一个<see cref="ICache"/>
    /// </summary>
    /// <typeparam name="TKey">缓存Key的对象</typeparam>
    /// <typeparam name="TValue">缓存值的对象</typeparam>
    public class TypedCacheWrapper<TKey, TValue> : ITypedCache<TKey, TValue>
    {
        /// <summary>
        /// 缓存的唯一名称
        /// </summary>
        public string Name
        {
            get { return InternalCache.Name; }
        }

        /// <summary>
        /// 缓存项的默认滑动过期时间
        /// </summary>
        public TimeSpan DefaultSlidingExpireTime
        {
            get { return InternalCache.DefaultSlidingExpireTime; }
            set { InternalCache.DefaultSlidingExpireTime = value; }
        }

        /// <summary>
        /// 获取内部缓存
        /// </summary>
        public ICache InternalCache { get; private set; }

        /// <summary>
        /// Creates a new <see cref="TypedCacheWrapper{TKey,TValue}"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="internalCache">The actual internal cache / 实际的内部缓存</param>
        public TypedCacheWrapper(ICache internalCache)
        {
            InternalCache = internalCache;
        }

        /// <summary>
        /// 释放缓存
        /// </summary>
        public void Dispose()
        {
            InternalCache.Dispose();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void Clear()
        {
            InternalCache.Clear();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            return InternalCache.ClearAsync();
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">如果不存在使用工厂方法创建缓存项</param>
        /// <returns>缓存项</returns>
        public TValue Get(TKey key, Func<TKey, TValue> factory)
        {
            return InternalCache.Get(key, factory);
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">如果不存在使用工厂方法创建缓存项</param>
        /// <returns>缓存项</returns>
        public Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory)
        {
            return InternalCache.GetAsync(key, factory);
        }

        /// <summary>
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存项目 / 没有找到则是null</returns>
        public TValue GetOrDefault(TKey key)
        {
            return InternalCache.GetOrDefault<TKey, TValue>(key);
        }

        /// <summary>
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存项目 / 没有找到则是null</returns>
        public Task<TValue> GetOrDefaultAsync(TKey key)
        {
            return InternalCache.GetOrDefaultAsync<TKey, TValue>(key);
        }

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        public void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null)
        {
            InternalCache.Set(key.ToString(), value, slidingExpireTime);
        }

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        public Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null)
        {
            return InternalCache.SetAsync(key.ToString(), value, slidingExpireTime);
        }

        /// <summary>
        /// 移除指定键的缓存(如果指定的key不存在缓存中，则啥事也不做)
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(TKey key)
        {
            InternalCache.Remove(key.ToString());
        }

        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public Task RemoveAsync(TKey key)
        {
            return InternalCache.RemoveAsync(key.ToString());
        }
    }
}