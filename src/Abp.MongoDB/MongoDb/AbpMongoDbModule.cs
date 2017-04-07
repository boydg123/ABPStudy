using System.Reflection;
using Abp.Modules;
using Abp.MongoDb.Configuration;

namespace Abp.MongoDb
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in MongoDB.
    /// 这个模块用于在MongoDB中实现数据访问层
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpMongoDbModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpMongoDbModuleConfiguration, AbpMongoDbModuleConfiguration>();
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
