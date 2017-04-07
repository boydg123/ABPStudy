using Abp.Dependency;
using Abp.Extensions;
using Quartz;
using Quartz.Spi;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// ABP Quartz Windsor 工厂
    /// </summary>
    public class AbpQuartzWindsorFactory : IJobFactory
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        public AbpQuartzWindsorFactory(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <summary>
        /// 获取一个Job
        /// </summary>
        /// <param name="bundle">触发器发射束?</param>
        /// <param name="scheduler">调度器</param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _iocResolver.Resolve(bundle.JobDetail.JobType).As<IJob>();
        }

        /// <summary>
        /// 解析Job
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            _iocResolver.Release(job);
        }
    }
}