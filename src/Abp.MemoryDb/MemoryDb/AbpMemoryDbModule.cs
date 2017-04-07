using System.Reflection;
using Abp.MemoryDb.Configuration;
using Abp.Modules;

namespace Abp.MemoryDb
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in MemoryDb.
    /// 这个模块用于在MemoryDB中实现数据访问层
    /// </summary>
    public class AbpMemoryDbModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpMemoryDbModuleConfiguration, AbpMemoryDbModuleConfiguration>();
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
