using System;
using Abp.Threading;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Some extension methods for <see cref="IBackgroundJobManager"/>.
    /// <see cref="IBackgroundJobManager"/>的扩展方法
    /// </summary>
    public static class BackgroundJobManagerExtensions
    {
        /// <summary>
        /// Enqueues a job to be executed.
        /// 入队的作业被执行
        /// </summary>
        /// <typeparam name="TJob">Type of the job. / 作业的类型</typeparam>
        /// <typeparam name="TArgs">Type of the arguments of job. / 作业参数的类型</typeparam>
        /// <param name="backgroundJobManager">Background job manager reference / 后台作业管理引用</param>
        /// <param name="args">Job arguments. / 作业参数</param>
        /// <param name="priority">Job priority. / 作业属性</param>
        /// <param name="delay">Job delay (wait duration before first try). / 作业延迟(初次前等待间隔)</param>
        public static void Enqueue<TJob, TArgs>(this IBackgroundJobManager backgroundJobManager, TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob<TArgs>
        {
            AsyncHelper.RunSync(() => backgroundJobManager.EnqueueAsync<TJob, TArgs>(args, priority, delay));
        }
    }
}
