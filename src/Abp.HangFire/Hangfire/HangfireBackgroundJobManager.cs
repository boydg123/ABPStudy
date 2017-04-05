using System;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Hangfire.Configuration;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using HangfireBackgroundJob = Hangfire.BackgroundJob;

namespace Abp.Hangfire
{
    /// <summary>
    /// Hangfire后台工作管理器
    /// </summary>
    public class HangfireBackgroundJobManager : BackgroundWorkerBase, IBackgroundJobManager
    {
        /// <summary>
        /// 后台作业配置
        /// </summary>
        private readonly IBackgroundJobConfiguration _backgroundJobConfiguration;

        /// <summary>
        /// ABP Hangfire配置
        /// </summary>
        private readonly IAbpHangfireConfiguration _hangfireConfiguration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="backgroundJobConfiguration">后台作业配置</param>
        /// <param name="hangfireConfiguration">ABP Hangfire配置</param>
        public HangfireBackgroundJobManager(
            IBackgroundJobConfiguration backgroundJobConfiguration,
            IAbpHangfireConfiguration hangfireConfiguration)
        {
            _backgroundJobConfiguration = backgroundJobConfiguration;
            _hangfireConfiguration = hangfireConfiguration;
        }

        /// <summary>
        /// 启动作业
        /// </summary>
        public override void Start()
        {
            base.Start();

            if (_hangfireConfiguration.Server == null && _backgroundJobConfiguration.IsJobExecutionEnabled)
            {
                _hangfireConfiguration.Server = new BackgroundJobServer();
            }
        }

        /// <summary>
        /// 等待作业停止
        /// </summary>
        public override void WaitToStop()
        {
            if (_hangfireConfiguration.Server != null)
            {
                try
                {
                    _hangfireConfiguration.Server.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            base.WaitToStop();
        }

        /// <summary>
        /// 队列执行任务
        /// </summary>
        /// <typeparam name="TJob">Job对象</typeparam>
        /// <typeparam name="TArgs">Job参数对象</typeparam>
        /// <param name="args">Job参数</param>
        /// <param name="priority">作业优先级</param>
        /// <param name="delay">延迟时间间隔</param>
        /// <returns></returns>
        public Task EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null) where TJob : IBackgroundJob<TArgs>
        {
            if (!delay.HasValue)
                HangfireBackgroundJob.Enqueue<TJob>(job => job.Execute(args));
            else
                HangfireBackgroundJob.Schedule<TJob>(job => job.Execute(args), delay.Value);
            return Task.FromResult(0);
        }
    }
}
