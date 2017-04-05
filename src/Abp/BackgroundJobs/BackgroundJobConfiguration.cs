using Abp.Configuration.Startup;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// 用于配置后台作业系统
    /// </summary>
    internal class BackgroundJobConfiguration : IBackgroundJobConfiguration
    {
        /// <summary>
        /// 用于 启用/禁用 后台作业的执行
        /// </summary>
        public bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        /// 获取ABP框架配置对象
        /// </summary>
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpConfiguration">ABP框架配置对象</param>
        public BackgroundJobConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;

            IsJobExecutionEnabled = true;
        }
    }
}