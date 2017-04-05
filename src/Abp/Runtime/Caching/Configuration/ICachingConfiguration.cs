using System;
using System.Collections.Generic;
using Abp.Configuration.Startup;

namespace Abp.Runtime.Caching.Configuration
{
    /// <summary>
    /// Used to configure caching system.
    /// 用于配置缓存系统
    /// </summary>
    public interface ICachingConfiguration
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// 获取ABP配置对象
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }

        /// <summary>
        /// List of all registered configurators.
        /// 所有注册的缓存配置列表
        /// </summary>
        IReadOnlyList<ICacheConfigurator> Configurators { get; }

        /// <summary>
        /// Used to configure all caches.
        /// 用于配置所有缓存
        /// </summary>
        /// <param name="initAction">
        /// An action to configure caches，This action is called for each cache just after created.
        /// 一个用于配置缓存的方法，该方法仅在缓存创建后被调用
        /// </param>
        void ConfigureAll(Action<ICache> initAction);

        /// <summary>
        /// Used to configure a specific cache. 
        /// 用于配置特定的缓存
        /// </summary>
        /// <param name="cacheName">Cache name / 缓存名称</param>
        /// <param name="initAction">
        /// An action to configure the cache.This action is called just after the cache is created.
        /// 一个用于配置缓存的方法，该方法仅在缓存创建后被调用
        /// </param>
        void Configure(string cacheName, Action<ICache> initAction);
    }
}
