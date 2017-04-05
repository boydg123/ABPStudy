using Abp.Configuration.Startup;

namespace Abp.AutoMapper
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Abp.AutoMapper module.
    /// <see cref="IModuleConfigurations"/>的扩展方法，用于允许配置Abp.AutoMapper模块
    /// </summary>
    public static class AbpWebApiConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Abp.AutoMapper module.
        /// 用于配置Abp.AutoMapper模块.
        /// </summary>
        public static IAbpAutoMapperConfiguration AbpAutoMapper(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpAutoMapperConfiguration>();
        }
    }
}