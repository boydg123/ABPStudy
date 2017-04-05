using Abp.BackgroundJobs;
using Abp.Modules;
using Hangfire;

namespace Abp.Hangfire.Configuration
{
    /// <summary>
    /// Used to configure Hangfire.
    /// 用于配置Hangfire
    /// </summary>
    public interface IAbpHangfireConfiguration
    {
        /// <summary>
        /// Gets or sets the Hanfgire's <see cref="BackgroundJobServer"/> object.
        /// Important: This is null in <see cref="AbpModule.PreInitialize"/>. You can create and set it to customize it's creation.
        /// If you don't set it, it's automatically set in <see cref="AbpModule.PreInitialize"/> by Abp.HangFire module with it's default constructor
        /// if background job execution is enabled (see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>).
        /// So, if you create it yourself, it's your responsibility to check if background job execution is enabled (see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>).
        /// 获取或设置Hangfire的<see cref="BackgroundJobServer"/>对象。
        /// 实现：这个在<see cref="AbpModule.PreInitialize"/>里为null，你可以创建并设置它来自定义它的创建。
        /// 如果你不能设置它，它会通过Abp.Hangfire模块的默认构造函数<see cref="AbpModule.PreInitialize"/>里自动设置，
        /// 如果后台作业的执行时可用的，查看(see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>)
        /// 因此，如果你自己创建它，你有责任检查后台作业执行是否被启用。(see <see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>).
        /// </summary>
        BackgroundJobServer Server { get; set; }

        /// <summary>
        /// A reference to Hangfire's global configuration.
        /// Hangfire的全局配置引用
        /// </summary>
        IGlobalConfiguration GlobalConfiguration { get; }
    }
}