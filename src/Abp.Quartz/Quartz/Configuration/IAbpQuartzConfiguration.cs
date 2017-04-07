using Quartz;

namespace Abp.Quartz.Quartz.Configuration
{
    /// <summary>
    /// ABP Quartz配置
    /// </summary>
    public interface IAbpQuartzConfiguration
    {
        /// <summary>
        /// 调度器
        /// </summary>
        IScheduler Scheduler { get;}
    }
}