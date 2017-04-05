namespace Abp.Notifications
{
    /// <summary>
    /// A class contains a <see cref="UserNotificationInfo"/> and related <see cref="NotificationInfo"/>.
    /// 一个包含<see cref="UserNotificationInfo"/>和关联<see cref="NotificationInfo"/>的类
    /// </summary>
    public class UserNotificationInfoWithNotificationInfo
    {
        /// <summary>
        /// User notification.
        /// 用户通知信息
        /// </summary>
        public UserNotificationInfo UserNotification { get; set; }

        /// <summary>
        /// Notification.
        /// 租户通知信息
        /// </summary>
        public TenantNotificationInfo Notification { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationInfoWithNotificationInfo"/> class.
        /// 初始化<see cref="UserNotificationInfoWithNotificationInfo"/>类新的实例
        /// </summary>
        public UserNotificationInfoWithNotificationInfo(UserNotificationInfo userNotification, TenantNotificationInfo notification)
        {
            UserNotification = userNotification;
            Notification = notification;
        }
    }
}