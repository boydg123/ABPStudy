using System.Reflection;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Hangfire;

namespace Abp.Hangfire
{
    /// <summary>
    /// Abp Hangfire模块。该模块依赖于<see cref="AbpKernelModule"/>
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpHangfireModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpHangfireConfiguration, AbpHangfireConfiguration>();
            
            Configuration.Modules
                .AbpHangfire()
                .GlobalConfiguration
                .UseActivator(new HangfireIocJobActivator(IocManager));
        }

        /// <summary>
        /// 初始化(这个方法用于模块的依赖注册)
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
