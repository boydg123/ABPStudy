using Abp.BackgroundJobs;
using Abp.Modules;
using Hangfire;
using HangfireGlobalConfiguration = Hangfire.GlobalConfiguration;

namespace Abp.Hangfire.Configuration
{
    /// <summary>
    /// <see cref="IAbpHangfireConfiguration"/>实现
    /// </summary>
    public class AbpHangfireConfiguration : IAbpHangfireConfiguration
    {
        /// <summary>
        /// 获取或设置Hangfire的<see cref="BackgroundJobServer"/>对象。
        /// 实现：这个在<see cref="AbpModule.PreInitialize"/>里为null，你可以创建并设置它来自定义它的创建。
        /// 如果你不能设置它，它会通过Abp.Hangfire模块的默认构造函数<see cref="AbpModule.PreInitialize"/>里自动设置，
        /// 如果后台作业的执行时可用的，查看(see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>)
        /// 因此，如果你自己创建它，你有责任检查后台作业执行是否被启用。(see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>).
        /// </summary>
        public BackgroundJobServer Server { get; set; }

        /// <summary>
        /// Hangfire的全局配置
        /// </summary>
        public IGlobalConfiguration GlobalConfiguration
        {
            get { return HangfireGlobalConfiguration.Configuration; }
        }
    }
}