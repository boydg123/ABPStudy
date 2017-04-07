using System;
using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Extension methods for <see cref="ICachingConfiguration"/>.
    /// <see cref="ICachingConfiguration"/>的扩展方法
    /// </summary>
    public static class RedisCacheConfigurationExtensions
    {
        /// <summary>
        /// Configures caching to use Redis as cache server.
        /// 使用Redis作为缓存服务来配置缓存
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration. / 缓存配置</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration)
        {
            cachingConfiguration.UseRedis(options => { });
        }

        /// <summary>
        /// Configures caching to use Redis as cache server.
        /// 使用Redis作为缓存服务来配置缓存
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration. / 缓存配置</param>
        /// <param name="optionsAction">Ac action to get/set options / 获取/设置选项的方法</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration, Action<AbpRedisCacheOptions> optionsAction)
        {
            var iocManager = cachingConfiguration.AbpConfiguration.IocManager;

            iocManager.RegisterIfNot<ICacheManager, AbpRedisCacheManager>();

            optionsAction(iocManager.Resolve<AbpRedisCacheOptions>());
        }
    }
}
