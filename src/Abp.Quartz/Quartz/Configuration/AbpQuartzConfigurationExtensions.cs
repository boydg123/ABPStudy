using System;

using Abp.BackgroundJobs;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Quartz.Quartz.Configuration
{
    /// <summary>
    /// ABP Quartz配置扩展
    /// </summary>
    public static class AbpQuartzConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Quartz module.
        /// 用于配置ABP Quartz模块
        /// </summary>
        public static IAbpQuartzConfiguration AbpQuartz(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpQuartzConfiguration>();
        }

        /// <summary>
        /// Configures to use Quartz for background job management.
        /// 使用Quartz作为后台作业管理的配置
        /// </summary>
        public static void UseQuartz(this IBackgroundJobConfiguration backgroundJobConfiguration, Action<IAbpQuartzConfiguration> configureAction = null)
        {
            backgroundJobConfiguration.AbpConfiguration.IocManager.RegisterIfNot<IQuartzScheduleJobManager, QuartzScheduleJobManager>();
            configureAction?.Invoke(backgroundJobConfiguration.AbpConfiguration.Modules.AbpQuartz());
        }
    }
}