using System;

namespace Abp.Notifications
{
    /// <summary>
    /// Arguments for <see cref="NotificationDistributionJob"/>.
    /// <see cref="NotificationDistributionJob"/>参数
    /// </summary>
    [Serializable]
    public class NotificationDistributionJobArgs
    {
        /// <summary>
        /// Notification Id.
        /// 通知ID
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDistributionJobArgs"/> class.
        /// 初始化<see cref="NotificationDistributionJobArgs"/>类新的实例
        /// </summary>
        public NotificationDistributionJobArgs(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}