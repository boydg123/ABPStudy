using System;
using Abp.Dependency;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Extension methods for <see cref="IAbpStartupConfiguration"/>.
    /// <see cref="IAbpStartupConfiguration"/>的扩展方法
    /// </summary>
    public static class AbpStartupConfigurationExtensions
    {
        /// <summary>
        /// Used to replace a service type.
        /// 用于替换一个服务类型
        /// </summary>
        /// <param name="configuration">The configuration. / ABP启动配置</param>
        /// <param name="type">Type. / 类型</param>
        /// <param name="impl">Implementation. / 类型实现</param>
        /// <param name="lifeStyle">Life style. / 生命周期</param>
        public static void ReplaceService(this IAbpStartupConfiguration configuration, Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
        {
            configuration.ReplaceService(type, () =>
            {
                configuration.IocManager.Register(type, impl, lifeStyle);
            });
        }

        /// <summary>
        /// Used to replace a service type.
        /// 用于替换一个服务类型
        /// </summary>
        /// <typeparam name="TType">Type of the service. / 服务类型</typeparam>
        /// <typeparam name="TImpl">Type of the implementation. / 类型的实现</typeparam>
        /// <param name="configuration">The configuration. / ABP启动配置</param>
        /// <param name="lifeStyle">Life style. / 生命周期</param>
        public static void ReplaceService<TType, TImpl>(this IAbpStartupConfiguration configuration, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TType : class
            where TImpl : class, TType
        {
            configuration.ReplaceService(typeof(TType), () =>
            {
                configuration.IocManager.Register<TType, TImpl>(lifeStyle);
            });
        }


        /// <summary>
        /// Used to replace a service type.
        /// 用于替换一个服务类型
        /// </summary>
        /// <typeparam name="TType">Type of the service. / 服务类型</typeparam>
        /// <param name="configuration">The configuration. / ABP启动配置</param>
        /// <param name="replaceAction">Replace action. / 替换方法</param>
        public static void ReplaceService<TType>(this IAbpStartupConfiguration configuration, Action replaceAction)
            where TType : class
        {
            configuration.ReplaceService(typeof(TType), replaceAction);
        }
    }
}