using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading;

namespace Abp.Notifications
{
    /// <summary>
    /// This background job distributes notifications to users.
    /// 后台作业向用户分发通知
    /// </summary>
    public class NotificationDistributionJob : BackgroundJob<NotificationDistributionJobArgs>, ITransientDependency
    {
        /// <summary>
        /// 用户分发通知
        /// </summary>
        private readonly INotificationDistributer _notificationDistributer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDistributionJob"/> class.
        /// 初始化<see cref="NotificationDistributionJob"/>类新的实例
        /// </summary>
        public NotificationDistributionJob(INotificationDistributer notificationDistributer)
        {
            _notificationDistributer = notificationDistributer;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="args">分发通知作业参数</param>
        public override void Execute(NotificationDistributionJobArgs args)
        {
            AsyncHelper.RunSync(() => _notificationDistributer.DistributeAsync(args.NotificationId));
        }
    }
}
