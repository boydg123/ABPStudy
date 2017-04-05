using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;
using JetBrains.Annotations;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Base class for cache managers.
    /// 缓存管理器的基类
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager, ISingletonDependency
    {
        /// <summary>
        /// IOC管理器
        /// </summary>
        protected readonly IIocManager IocManager;

        /// <summary>
        /// 缓存配置
        /// </summary>
        protected readonly ICachingConfiguration Configuration;

        /// <summary>
        /// 缓存对象字典
        /// </summary>
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="iocManager"></param>
        /// <param name="configuration"></param>
        protected CacheManagerBase(IIocManager iocManager, ICachingConfiguration configuration)
        {
            IocManager = iocManager;
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        /// <summary>
        /// 获取所有的缓存
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        /// <summary>
        /// 获取一个<see cref="ICache"/>实例，它可以创建缓存如果缓存不存在
        /// </summary>
        /// <param name="name">缓存的唯一名称(区分大小写)</param>
        /// <returns>缓存引用</returns>
        public virtual ICache GetCache(string name)
        {
            Check.NotNull(name, nameof(name));

            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);

                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }

                return cache;
            });
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var cache in Caches)
            {
                IocManager.Release(cache.Value);
            }

            Caches.Clear();
        }

        /// <summary>
        /// Used to create actual cache implementation.
        /// 用于创建实际缓存实现
        /// </summary>
        /// <param name="name">Name of the cache / 缓存的名称</param>
        /// <returns>Cache object / 缓存对象引用</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}