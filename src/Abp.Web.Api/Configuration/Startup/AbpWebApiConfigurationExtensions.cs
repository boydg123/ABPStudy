using Abp.WebApi.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Abp.Web.Api module.
    /// 定义<see cref="IModuleConfigurations"/>扩展方法用于允许配置 Abp.Web.Api 模块
    /// </summary>
    public static class AbpWebApiConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Abp.Web.Api module.
        /// 用于配置 Abp.Web.Api 模块
        /// </summary>
        public static IAbpWebApiConfiguration AbpWebApi(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpWebApiConfiguration>();
        }
    }
}