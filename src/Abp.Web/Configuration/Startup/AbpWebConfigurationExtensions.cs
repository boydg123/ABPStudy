using Abp.Web.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP Web module.
    /// 定义<see cref="IModuleConfigurations"/>的扩展方法，允许配置ABP Web模块
    /// </summary>
    public static class AbpWebConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Web module.
        /// 用于配置ABP Web模块
        /// </summary>
        public static IAbpWebModuleConfiguration AbpWeb(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpWebModuleConfiguration>();
        }
    }
}