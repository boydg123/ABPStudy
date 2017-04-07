using System;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Quartz.Quartz.Configuration;
using Abp.Threading.BackgroundWorkers;
using Quartz;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// Quartz作业调度管理器
    /// </summary>
    public class QuartzScheduleJobManager : BackgroundWorkerBase, IQuartzScheduleJobManager
    {
        /// <summary>
        /// 后台作业配置
        /// </summary>
        private readonly IBackgroundJobConfiguration _backgroundJobConfiguration;

        /// <summary>
        /// ABP Quartz配置
        /// </summary>
        private readonly IAbpQuartzConfiguration _quartzConfiguration;

        public QuartzScheduleJobManager(
            IAbpQuartzConfiguration quartzConfiguration, 
            IBackgroundJobConfiguration backgroundJobConfiguration)
        {
            _quartzConfiguration = quartzConfiguration;
            _backgroundJobConfiguration = backgroundJobConfiguration;
        }

        public Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger)
            where TJob : IJob
        {
            var jobToBuild = JobBuilder.Create<TJob>();
            configureJob(jobToBuild);
            var job = jobToBuild.Build();

            var triggerToBuild = TriggerBuilder.Create();
            configureTrigger(triggerToBuild);
            var trigger = triggerToBuild.Build();

            _quartzConfiguration.Scheduler.ScheduleJob(job, trigger);

            return Task.FromResult(0);
        }

        public override void Start()
        {
            base.Start();

            if (_backgroundJobConfiguration.IsJobExecutionEnabled)
            {
                _quartzConfiguration.Scheduler.Start();
            }
        }

        public override void WaitToStop()
        {
            if (_quartzConfiguration.Scheduler != null)
            {
                try
                {
                    _quartzConfiguration.Scheduler.Shutdown(true);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            base.WaitToStop();
        }
    }
}