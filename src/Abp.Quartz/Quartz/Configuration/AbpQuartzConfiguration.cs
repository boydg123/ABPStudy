using Quartz;
using Quartz.Impl;

namespace Abp.Quartz.Quartz.Configuration
{
    /// <summary>
    /// ABP Quartz配置
    /// </summary>
    public class AbpQuartzConfiguration : IAbpQuartzConfiguration
    {
        /// <summary>
        /// 获取调度
        /// </summary>
        public IScheduler Scheduler => StdSchedulerFactory.GetDefaultScheduler();
    }
}