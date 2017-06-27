using System;
using System.Web.OData.Builder;
using Abp.Configuration.Startup;

namespace Abp.WebApi.OData.Configuration
{
    /// <summary>
    /// Used to configure Abp.Web.Api.OData module.
    /// 用于配置Abp.Web.Api.Odata模块
    /// </summary>
    public interface IAbpWebApiODataModuleConfiguration
    {
        /// <summary>
        /// Gets ODataConventionModelBuilder.
        /// 获取 ODataConventionModelBuilder
        /// </summary>
        ODataConventionModelBuilder ODataModelBuilder { get; set; }

        /// <summary>
        /// Allows overriding OData mapping.
        /// 允许重写OData映射
        /// </summary>
        Action<IAbpStartupConfiguration> MapAction { get; set; }
    }
}