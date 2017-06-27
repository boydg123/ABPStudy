using System.Reflection;
using System.Web.OData;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Modules;
using Abp.WebApi.OData.Configuration;

namespace Abp.WebApi.OData
{
    /// <summary>
    /// Abp WebApi OData模块，该模块依赖于<see cref="AbpWebApiModule"/>
    /// </summary>
    [DependsOn(typeof(AbpWebApiModule))]
    public class AbpWebApiODataModule : AbpModule
    {
        /// <summary>
        /// 初始化前
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpWebApiODataModuleConfiguration, AbpWebApiODataModuleConfiguration>();

            Configuration.Validation.IgnoredTypes.AddIfNotContains(typeof(Delta));
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        public override void Initialize()
        {
            IocManager.Register<MetadataController>(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApiOData().MapAction?.Invoke(Configuration);
        }
    }
}
