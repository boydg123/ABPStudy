using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Abp.Auditing;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.Web.Session;
using Abp.Configuration.Startup;
using Abp.Web.Configuration;
using Abp.Web.Security.AntiForgery;
using Abp.Collections.Extensions;
using Abp.Dependency;

namespace Abp.Web
{
    /// <summary>
    /// This module is used to use ABP in ASP.NET web applications.
    /// 此模块用于在ASP.NET Web应用程序中使用ABP
    /// </summary>
    [DependsOn(typeof(AbpWebCommonModule))]    
    public class AbpWebModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpAntiForgeryWebConfiguration, AbpAntiForgeryWebConfiguration>();
            IocManager.Register<IAbpWebLocalizationConfiguration, AbpWebLocalizationConfiguration>();
            IocManager.Register<IAbpWebModuleConfiguration, AbpWebModuleConfiguration>();
            
            Configuration.ReplaceService<IPrincipalAccessor, HttpContextPrincipalAccessor>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IClientInfoProvider, WebAuditInfoProvider>(DependencyLifeStyle.Transient);

            AddIgnoredTypes();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());            
        }

        /// <summary>
        /// 添加忽略类型
        /// </summary>
        private void AddIgnoredTypes()
        {
            var ignoredTypes = new[]
            {
                typeof(HttpPostedFileBase),
                typeof(IEnumerable<HttpPostedFileBase>),
                typeof(HttpPostedFileWrapper),
                typeof(IEnumerable<HttpPostedFileWrapper>)
            };
            
            foreach (var ignoredType in ignoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }
    }
}
