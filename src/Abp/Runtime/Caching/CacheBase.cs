using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Base class for caches.It's used to simplify implementing <see cref="ICache"/>.
    /// 缓存基类，它用作<see cref="ICache"/>简单的实现
    /// </summary>
    public abstract class CacheBase : ICache
    {
        /// <summary>
        /// 缓存的唯一名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 默认滑动过期时间
        /// </summary>
        public TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// 默认绝对过期时间
        /// </summary>
        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        /// <summary>
        /// 同步对象
        /// </summary>
        protected readonly object SyncObj = new object();

        private readonly AsyncLock _asyncLock = new AsyncLock();

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="name">缓存的唯一名称</param>
        protected CacheBase(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">如果不存在使用工厂方法创建缓存项</param>
        /// <returns>缓存项</returns>
        public virtual object Get(string key, Func<string, object> factory)
        {
            var cacheKey = key;
            var item = GetOrDefault(key);
            if (item == null)
            {
                lock (SyncObj)
                {
                    item = GetOrDefault(key);
                    if (item == null)
                    {
                        item = factory(key);
                        if (item == null)
                        {
                            return null;
                        }

                        Set(cacheKey, item);
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">如果不存在使用工厂方法创建缓存项</param>
        /// <returns></returns>
        public virtual async Task<object> GetAsync(string key, Func<string, Task<object>> factory)
        {
            var cacheKey = key;
            var item = await GetOrDefaultAsync(key);
            if (item == null)
            {
                using (await _asyncLock.LockAsync())
                {
                    item = await GetOrDefaultAsync(key);
                    if (item == null)
                    {
                        item = await factory(key);
                        if (item == null)
                        {
                            return null;
                        }

                        await SetAsync(cacheKey, item);
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// 将被子类重写。从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public abstract object GetOrDefault(string key);

        /// <summary>
        /// 将被子类重写。从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.FromResult(GetOrDefault(key));
        }

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public abstract void Remove(string key);

        /// <summary>
        /// 移除指定键的缓存(如果指定的key不存在缓存中，则啥事也不做)
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 清除所有的缓存数据
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 清除所有的缓存数据 - 异步
        /// </summary>
        /// <returns></returns>
        public virtual Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}