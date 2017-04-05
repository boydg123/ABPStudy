using Abp.Configuration.Startup;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Used to configure background job system.
    /// 用于配置后台作业系统
    /// </summary>
    public interface IBackgroundJobConfiguration
    {
        /// <summary>
        /// Used to enable/disable background job execution.
        /// 用于 启用/禁用 后台作业的执行
        /// </summary>
        bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        /// Gets the ABP configuration object.
        /// 获取ABP框架配置对象
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }
    }
}