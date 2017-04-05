using System;

namespace Abp.Runtime.Caching.Configuration
{
    /// <summary>
    /// A registered cache configurator.
    /// 注册缓存配置其
    /// </summary>
    public interface ICacheConfigurator
    {
        /// <summary>
        /// Name of the cache.It will be null if this configurator configures all caches.
        /// 缓存名称，它将为null，如果这个配置其配置所有缓存
        /// </summary>
        string CacheName { get; }

        /// <summary>
        /// Configuration action. Called just after the cache is created.
        /// 配置方法,紧紧在缓存创建后调用
        /// </summary>
        Action<ICache> InitAction { get; }
    }
}