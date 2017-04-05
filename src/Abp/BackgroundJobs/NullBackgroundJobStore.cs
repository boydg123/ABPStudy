using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Null pattern implementation of <see cref="IBackgroundJobStore"/>.
    /// <see cref="IBackgroundJobStore"/>的NULL模式实现
    /// It's used if <see cref="IBackgroundJobStore"/> is not implemented by actual persistent store
    /// 它被使用，如果<see cref="IBackgroundJobStore"/>没有被持久化存储实现
    /// and job execution is not enabled (<see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>) for the application.
    /// 并且应用程序作业的执行是禁用的(<see cref="IBackgroundJobConfiguration.IsJobExecutionEnabled"/>)。
    /// </summary>
    public class NullBackgroundJobStore : IBackgroundJobStore
    {
        /// <summary>
        /// 异步插入一个后台作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task InsertAsync(BackgroundJobInfo jobInfo)
        {
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
            return Task.FromResult(new List<BackgroundJobInfo>());
        }

        /// <summary>
        /// 删除一个作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task DeleteAsync(BackgroundJobInfo jobInfo)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 更新一个作业
        /// </summary>
        /// <param name="jobInfo">作业信息</param>
        /// <returns></returns>
        public Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            return Task.FromResult(0);
        }
    }
}