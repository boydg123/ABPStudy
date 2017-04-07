using System.Reflection;
using Abp.Modules;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// This modules is used to replace ABP's cache system with Redis server.
    /// 此模块用于使用Redis服务替换ABP的缓存系统
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpRedisCacheModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<AbpRedisCacheOptions>();
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
