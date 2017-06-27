using System;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Abp.Configuration.Startup;

namespace Abp.WebApi.OData.Configuration
{
    /// <summary>
    /// <see cref="IAbpWebApiODataModuleConfiguration"/>的实现
    /// </summary>
    internal class AbpWebApiODataModuleConfiguration : IAbpWebApiODataModuleConfiguration
    {
        public ODataConventionModelBuilder ODataModelBuilder { get; set; }

        /// <summary>
        /// 返回<see cref="IAbpStartupConfiguration"/>的Action
        /// </summary>
        public Action<IAbpStartupConfiguration> MapAction { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpWebApiODataModuleConfiguration()
        {
            ODataModelBuilder = new ODataConventionModelBuilder();

            MapAction = configuration =>
            {
                configuration.Modules.AbpWebApi().HttpConfiguration.MapODataServiceRoute(
                    routeName: "ODataRoute",
                    routePrefix: "odata",
                    model: configuration.Modules.AbpWebApiOData().ODataModelBuilder.GetEdmModel()
                );
            };
        }
    }
}