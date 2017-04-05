using System;
using System.Runtime.Caching;

namespace Abp.Runtime.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICache"/> to work with <see cref="MemoryCache"/>.
    /// 使用<see cref="MemoryCache"/>实现<see cref="ICache"/>
    /// </summary>
    public class AbpMemoryCache : CacheBase
    {
        /// <summary>
        /// 内存缓存对象
        /// </summary>
        private MemoryCache _memoryCache;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="name">Unique name of the cache</param>
        public AbpMemoryCache(string name)
            : base(name)
        {
            _memoryCache = new MemoryCache(Name);
        }

        /// <summary>
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new AbpException("Can not insert null values to the cache!");
            }

            var cachePolicy = new CacheItemPolicy();

            if (absoluteExpireTime != null)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpireTime.Value);
            }
            else if (slidingExpireTime != null)
            {
                cachePolicy.SlidingExpiration = slidingExpireTime.Value;
            }
            else if(DefaultAbsoluteExpireTime != null)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value);
            }
            else
            {
                cachePolicy.SlidingExpiration = DefaultSlidingExpireTime;
            }

            _memoryCache.Set(key, value, cachePolicy);
        }

        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// 清除所有的缓存数据
        /// </summary>
        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(Name);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}