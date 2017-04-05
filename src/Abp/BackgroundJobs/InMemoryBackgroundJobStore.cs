using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Timing;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// In memory implementation of <see cref="IBackgroundJobStore"/>.
    /// 在内存中实现<see cref="IBackgroundJobStore"/>
    /// It's used if <see cref="IBackgroundJobStore"/> is not implemented by actual persistent store
    /// 它被使用，如果<see cref="IBackgroundJobStore"/>没有被持久化存储实现
    /// and job execution is enabled (<see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>) for the application.
    /// 并且应用程序作业的执行是启用的(<see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>)。
    /// </summary>
    public class InMemoryBackgroundJobStore : IBackgroundJobStore
    {
        /// <summary>
        /// 后台作业信息字典存储
        /// </summary>
        private readonly Dictionary<long, BackgroundJobInfo> _jobs;
        private long _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryBackgroundJobStore"/> class.
        /// 构造函数(初始化<see cref="InMemoryBackgroundJobStore"/>类新的实例)
        /// </summary>
        public InMemoryBackgroundJobStore()
        {
            _jobs = new Dictionary<long, BackgroundJobInfo>();
        }

        /// <summary>
        /// 异步插入一个后台作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            jobInfo.Id = Interlocked.Increment(ref _lastId);
            _jobs[jobInfo.Id] = jobInfo;

            return Task.FromResult(0);
        }

        /// <summary>
        /// 获取等待作业.它应该基于如下获取:
        /// 条件:!IsAbandoned And NextTryTime 小于等于 Clock.Now.
        /// 排序:Priority DESC, TryCount ASC, NextTryTime ASC.
        /// 最大结果:<paramref name="maxResultCount"/>.
        /// </summary>
        /// <param name="maxResultCount">最大结果数量</param>
        /// <returns></returns>
        public Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            var waitingJobs = _jobs.Values
                .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount)
                .ToList();

            return Task.FromResult(waitingJobs);
        }

        /// <summary>
        /// 删除一个作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task DeleteAsync(BackgroundJobInfo jobInfo)
        {
            if (!_jobs.ContainsKey(jobInfo.Id))
            {
                return Task.FromResult(0);
            }

            _jobs.Remove(jobInfo.Id);

            return Task.FromResult(0);
        }

        /// <summary>
        /// 更新一个作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            if (jobInfo.IsAbandoned)
            {
                return DeleteAsync(jobInfo);
            }

            return Task.FromResult(0);
        }
    }
}