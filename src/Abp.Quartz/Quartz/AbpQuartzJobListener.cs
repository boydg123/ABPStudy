using Castle.Core.Logging;

using Quartz;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// ABP Quartz 作业监听器
    /// </summary>
    public class AbpQuartzJobListener : IJobListener
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpQuartzJobListener()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 作业执行否决
        /// </summary>
        /// <param name="context">作业执行上下文</param>
        public virtual void JobExecutionVetoed(IJobExecutionContext context)
        {
            Logger.Info($"Job {context.JobDetail.JobType.Name} executing operation vetoed...");
        }

        /// <summary>
        /// 被执行的作业
        /// </summary>
        /// <param name="context">作业执行上下文</param>
        public virtual void JobToBeExecuted(IJobExecutionContext context)
        {
            Logger.Info($"Job {context.JobDetail.JobType.Name} executing...");
        }

        /// <summary>
        /// 作业执行
        /// </summary>
        /// <param name="context">作业执行上下文</param>
        /// <param name="jobException">作业执行异常</param>
        public virtual void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (jobException == null)
            {
                Logger.Info($"Job {context.JobDetail.JobType.Name} sucessfully executed.");
            }
            else
            {
                Logger.Error($"Job {context.JobDetail.JobType.Name} failed with exception:{jobException}");
            }
        }

        public string Name { get; } = "AbpJobListener";
    }
}