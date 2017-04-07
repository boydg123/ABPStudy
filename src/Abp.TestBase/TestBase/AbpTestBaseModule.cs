using System.Reflection;
using Abp.Modules;

namespace Abp.TestBase
{
    /// <summary>
    /// ABP Test Base 模块
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpTestBaseModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        /// <summary>
        /// 这个方法用于模块的依赖注册
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}