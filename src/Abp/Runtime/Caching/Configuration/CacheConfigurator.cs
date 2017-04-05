using System;

namespace Abp.Runtime.Caching.Configuration
{
    /// <summary>
    /// 缓存配置器
    /// </summary>
    internal class CacheConfigurator : ICacheConfigurator
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public string CacheName { get; private set; }

        /// <summary>
        /// 配置方法,紧紧在缓存创建后调用
        /// </summary>
        public Action<ICache> InitAction { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initAction">配置方法,紧紧在缓存创建后调用</param>
        public CacheConfigurator(Action<ICache> initAction)
        {
            InitAction = initAction;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="initAction">配置方法,紧紧在缓存创建后调用</param>
        public CacheConfigurator(string cacheName, Action<ICache> initAction)
        {
            CacheName = cacheName;
            InitAction = initAction;
        }
    }
}