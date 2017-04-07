using System.Reflection;

using Abp.Dependency;
using Abp.Modules;
using Abp.Quartz.Quartz.Configuration;
using Abp.Threading.BackgroundWorkers;

using Quartz;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// ABP Quartz模块
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpQuartzModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpQuartzConfiguration, AbpQuartzConfiguration>();
            IocManager.RegisterIfNot<IJobListener, AbpQuartzJobListener>();

            Configuration.Modules
                         .AbpQuartz()
                         .Scheduler
                         .JobFactory = new AbpQuartzWindsorFactory(IocManager);

            Configuration.Modules
                         .AbpQuartz()
                         .Scheduler
                         .ListenerManager.AddJobListener(IocManager.Resolve<IJobListener>());
        }

        /// <summary>
        /// 这个方法用于模块的依赖注册
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 这个方法在应用启动最后被调用
        /// </summary>
        public override void PostInitialize()
        {
            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workerManager.Add(IocManager.Resolve<IQuartzScheduleJobManager>());
            }
        }
    }
}