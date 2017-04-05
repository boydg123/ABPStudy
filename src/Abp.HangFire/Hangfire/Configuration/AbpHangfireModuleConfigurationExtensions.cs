using System;
using Abp.BackgroundJobs;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Hangfire.Configuration
{
    /// <summary>
    /// Abp Hangfire配置扩展方法
    /// </summary>
    public static class AbpHangfireConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Hangfire module.
        /// 用于配置ABP Hangfire模块
        /// </summary>
        public static IAbpHangfireConfiguration AbpHangfire(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpHangfireConfiguration>();
        }

        /// <summary>
        /// Configures to use Hangfire for background job management.
        /// 配置用于使用Hangfire的后台作业管理
        /// </summary>
        public static void UseHangfire(this IBackgroundJobConfiguration backgroundJobConfiguration, Action<IAbpHangfireConfiguration> configureAction)
        {
            backgroundJobConfiguration.AbpConfiguration.ReplaceService<IBackgroundJobManager, HangfireBackgroundJobManager>();
            configureAction(backgroundJobConfiguration.AbpConfiguration.Modules.AbpHangfire());
        }
    }
}