using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Timing;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Implements <see cref="IBackgroundJobStore"/> using repositories.
    /// 使用仓储实现<see cref="IBackgroundJobStore"/>
    /// </summary>
    public class BackgroundJobStore : IBackgroundJobStore, ITransientDependency
    {
        /// <summary>
        /// 后台作业仓储引用
        /// </summary>
        private readonly IRepository<BackgroundJobInfo, long> _backgroundJobRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="backgroundJobRepository">后台作业仓储</param>
        public BackgroundJobStore(IRepository<BackgroundJobInfo, long> backgroundJobRepository)
        {
            _backgroundJobRepository = backgroundJobRepository;
        }
        /// <summary>
        /// 插入一个Job
        /// </summary>
        /// <param name="jobInfo">Job信息</param>
        /// <returns></returns>
        public Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            return _backgroundJobRepository.InsertAsync(jobInfo);
        }
        /// <summary>
        /// 获取等待的Job信息集合
        /// </summary>
        /// <param name="maxResultCount">最大结果数</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            var waitingJobs = _backgroundJobRepository.GetAll()
                .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount)
                .ToList();

            return Task.FromResult(waitingJobs);
        }
        /// <summary>
        /// 删除一个Job
        /// </summary>
        /// <param name="jobInfo">Job信息</param>
        /// <returns></returns>
        public Task DeleteAsync(BackgroundJobInfo jobInfo)
        {
            return _backgroundJobRepository.DeleteAsync(jobInfo);
        }
        /// <summary>
        /// 修改一个Job
        /// </summary>
        /// <param name="jobInfo">Job信息</param>
        /// <returns></returns>
        public Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            return _backgroundJobRepository.UpdateAsync(jobInfo);
        }
    }
}
