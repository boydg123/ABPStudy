using System;
using System.Threading.Tasks;
using Abp.Threading.BackgroundWorkers;

namespace Abp.BackgroundJobs
{
    //TODO: Create a non-generic EnqueueAsync extension method to IBackgroundJobManager which takes types as input parameters rather than generic parameters.
    /// <summary>
    /// Defines interface of a job manager.
    /// 一个作业管理类接口
    /// </summary>
    public interface IBackgroundJobManager : IBackgroundWorker
    {
        /// <summary>
        /// Enqueues a job to be executed.
        /// 入队的作业被执行
        /// </summary>
        /// <typeparam name="TJob">Type of the job. / 作业的类型</typeparam>
        /// <typeparam name="TArgs">Type of the arguments of job. / 作业参数的类型</typeparam>
        /// <param name="args">Job arguments. / 作业参数</param>
        /// <param name="priority">Job priority. / 作业属性</param>
        /// <param name="delay">Job delay (wait duration before first try). / 作业延迟(初次前等待间隔)</param>
        Task EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob<TArgs>;
    }
}