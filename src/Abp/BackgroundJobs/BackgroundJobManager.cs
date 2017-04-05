using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Json;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using Newtonsoft.Json;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Default implementation of <see cref="IBackgroundJobManager"/>.
    /// <see cref="IBackgroundJobManager"/>的默认实现
    /// </summary>
    public class BackgroundJobManager : PeriodicBackgroundWorkerBase, IBackgroundJobManager, ISingletonDependency
    {
        /// <summary>
        /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.Default value: 5000 (5 seconds).
        /// 轮训作业之间的间隔<see cref="IBackgroundJobStore"/>.默认值:5000(5秒)
        /// </summary>
        public static int JobPollPeriod { get; set; }

        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 后台作业的存储引用
        /// </summary>
        private readonly IBackgroundJobStore _store;

        /// <summary>
        /// 构造函数
        /// </summary>
        static BackgroundJobManager()
        {
            JobPollPeriod = 5000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// 构造函数(初始化<see cref="BackgroundJobManager"/>类新的实例)
        /// </summary>
        public BackgroundJobManager(
            IIocResolver iocResolver,
            IBackgroundJobStore store,
            AbpTimer timer)
            : base(timer)
        {
            _store = store;
            _iocResolver = iocResolver;

            Timer.Period = JobPollPeriod;
        }

        /// <summary>
        /// Enqueues a job to be executed.
        /// 入队的作业被执行
        /// </summary>
        /// <typeparam name="TJob">Type of the job. / 作业的类型</typeparam>
        /// <typeparam name="TArgs">Type of the arguments of job. / 作业参数的类型</typeparam>
        /// <param name="args">Job arguments. / 作业参数</param>
        /// <param name="priority">Job priority. / 作业属性</param>
        /// <param name="delay">Job delay (wait duration before first try). / 作业延迟(初次前等待间隔)</param>
        public async Task EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob<TArgs>
        {
            var jobInfo = new BackgroundJobInfo
            {
                JobType = typeof(TJob).AssemblyQualifiedName,
                JobArgs = args.ToJsonString(),
                Priority = priority
            };

            if (delay.HasValue)
            {
                jobInfo.NextTryTime = Clock.Now.Add(delay.Value);
            }

            await _store.InsertAsync(jobInfo);
        }

        /// <summary>
        /// 定期工作应工作通过实现此方法
        /// </summary>
        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => _store.GetWaitingJobsAsync(1000));

            foreach (var job in waitingJobs)
            {
                TryProcessJob(job);
            }
        }

        /// <summary>
        /// 尝试进程作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        private void TryProcessJob(BackgroundJobInfo jobInfo)
        {
            try
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = Clock.Now;

                var jobType = Type.GetType(jobInfo.JobType);
                using (var job = _iocResolver.ResolveAsDisposable(jobType))
                {
                    try
                    {
                        var jobExecuteMethod = job.Object.GetType().GetMethod("Execute");
                        var argsType = jobExecuteMethod.GetParameters()[0].ParameterType;
                        var argsObj = JsonConvert.DeserializeObject(jobInfo.JobArgs, argsType);

                        jobExecuteMethod.Invoke(job.Object, new[] { argsObj });

                        AsyncHelper.RunSync(() => _store.DeleteAsync(jobInfo));
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.Message, ex);

                        var nextTryTime = jobInfo.CalculateNextTryTime();
                        if (nextTryTime.HasValue)
                        {
                            jobInfo.NextTryTime = nextTryTime.Value;
                        }
                        else
                        {
                            jobInfo.IsAbandoned = true;
                        }

                        TryUpdate(jobInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);

                jobInfo.IsAbandoned = true;

                TryUpdate(jobInfo);
            }
        }

        /// <summary>
        /// 尝试更新
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        private void TryUpdate(BackgroundJobInfo jobInfo)
        {
            try
            {
                _store.UpdateAsync(jobInfo);
            }
            catch (Exception updateEx)
            {
                Logger.Warn(updateEx.ToString(), updateEx);
            }
        }
    }
}
