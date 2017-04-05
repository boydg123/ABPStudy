using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// An upper level container for <see cref="ICache"/> objects.
    /// <see cref="ICache"/>对象的上一级容器。
    /// A cache manager should work as Singleton and track and manage <see cref="ICache"/> objects.
    /// 一个缓存管理器应该工作于单例模式跟踪和管理<see cref="ICache"/>对象
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// Gets all caches.
        /// 获取所有缓存
        /// </summary>
        /// <returns>List of caches / 缓存列表</returns>
        IReadOnlyList<ICache> GetAllCaches();

        /// <summary>
        /// Gets a <see cref="ICache"/> instance.It may create the cache if it does not already exists.
        /// 获取一个<see cref="ICache"/>实例，它可以创建缓存如果缓存不存在
        /// </summary>
        /// <param name="name">
        /// Unique and case sensitive name of the cache.
        /// 缓存的唯一名称(区分大小写)
        /// </param>
        /// <returns>The cache reference / 缓存引用</returns>
        [NotNull] ICache GetCache([NotNull] string name);
    }
}
