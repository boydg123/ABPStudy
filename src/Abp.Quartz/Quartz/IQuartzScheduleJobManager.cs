using System;
using System.Threading.Tasks;
using Abp.Threading.BackgroundWorkers;
using Quartz;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// Defines interface of Quartz schedule job manager.
    /// 定义Quartz调度作业管理的接口
    /// </summary>
    public interface IQuartzScheduleJobManager : IBackgroundWorker
    {
        /// <summary>
        /// Schedules a job to be executed.
        /// 调度一个作业来执行
        /// </summary>
        /// <typeparam name="TJob">Type of the job / 作业的类型</typeparam>
        /// <param name="configureJob">Job specific definitions to build. / 指定的作业定义</param>
        /// <param name="configureTrigger">Job specific trigger options which means calendar or time interval. / Job指定的触发器选项(意味着日历或者时间间隔)</param>
        /// <returns></returns>
        Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob;
    }
}