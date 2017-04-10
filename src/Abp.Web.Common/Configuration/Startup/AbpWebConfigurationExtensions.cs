using Abp.Web.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP Web module.
    /// <see cref="IModuleConfigurations"/>的扩展方法用于允许配置ABP Web模块
    /// </summary>
    public static class AbpWebConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Web Common module.
        /// 用于配置ABP Web Common模块
        /// </summary>
        public static IAbpWebCommonModuleConfiguration AbpWebCommon(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpWebCommonModuleConfiguration>();
        }
    }
}