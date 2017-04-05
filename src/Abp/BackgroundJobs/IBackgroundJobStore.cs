using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Defines interface to store/get background jobs.
    /// 存储/获取 后台作业的接口
    /// </summary>
    public interface IBackgroundJobStore
    {
        /// <summary>
        /// Inserts a background job.
        /// 异步插入一个后台作业
        /// </summary>
        /// <param name="jobInfo">Job information. / 作业信息</param>
        Task InsertAsync(BackgroundJobInfo jobInfo);

        /// <summary>
        /// Gets waiting jobs. It should get jobs based on these:
        /// 获取等待作业.它应该基于如下获取:
        /// Conditions: !IsAbandoned And NextTryTime &lt;= Clock.Now.
        /// 条件:!IsAbandoned And NextTryTime 小于等于 Clock.Now.
        /// Order by: Priority DESC, TryCount ASC, NextTryTime ASC.
        /// 排序:Priority DESC, TryCount ASC, NextTryTime ASC.
        /// Maximum result: <paramref name="maxResultCount"/>.
        /// 最大结果:<paramref name="maxResultCount"/>.
        /// </summary>
        /// <param name="maxResultCount">Maximum result count. / 最大结果数量</param>
        Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount);

        /// <summary>
        /// Deletes a job.
        /// 删除一个作业
        /// </summary>
        /// <param name="jobInfo">Job information. / 作业信息</param>
        Task DeleteAsync(BackgroundJobInfo jobInfo);

        /// <summary>
        /// Updates a job.
        /// 更新一个作业
        /// </summary>
        /// <param name="jobInfo">Job information. / 作业信息</param>
        Task UpdateAsync(BackgroundJobInfo jobInfo);
    }
}