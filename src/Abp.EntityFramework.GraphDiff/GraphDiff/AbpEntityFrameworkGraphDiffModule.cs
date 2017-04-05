using System.Collections.Generic;
using System.Reflection;
using Abp.EntityFramework.GraphDiff.Configuration;
using Abp.EntityFramework.GraphDiff.Mapping;
using Abp.Modules;

namespace Abp.EntityFramework.GraphDiff
{
    /// <summary>
    /// Abp EF Graph Diff模块。该模块依赖于<see cref="AbpEntityFrameworkModule"/> 和 <see cref="AbpKernelModule"/> 
    /// </summary>
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(AbpKernelModule))]
    public class AbpEntityFrameworkGraphDiffModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpEntityFrameworkGraphDiffModuleConfiguration, AbpEntityFrameworkGraphDiffModuleConfiguration>();

            Configuration.Modules
                .AbpEfGraphDiff()
                .UseMappings(new List<EntityMapping>());
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
