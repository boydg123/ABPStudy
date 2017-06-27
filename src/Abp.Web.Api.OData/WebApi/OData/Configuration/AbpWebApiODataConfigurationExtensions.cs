using Abp.Configuration.Startup;

namespace Abp.WebApi.OData.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure Abp.Web.Api.OData module.
    /// <see cref="IModuleConfigurations"/>的扩展方法用于允许配置Abp.Web.Api.OData模块
    /// </summary>
    public static class AbpWebApiODataConfigurationExtensions
    {
        /// <summary>
        /// Used to configure Abp.Web.Api.OData module.
        /// 用于配置Abp.Web.Api.OData模块
        /// </summary>
        public static IAbpWebApiODataModuleConfiguration AbpWebApiOData(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpWebApiODataModuleConfiguration>();
        }
    }
}