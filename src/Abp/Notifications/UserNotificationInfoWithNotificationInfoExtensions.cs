namespace Abp.Notifications
{
    /// <summary>
    /// Extension methods for <see cref="UserNotificationInfoWithNotificationInfo"/>.
    /// <see cref="UserNotificationInfoWithNotificationInfo"/>的扩展方法
    /// </summary>
    public static class UserNotificationInfoWithNotificationInfoExtensions
    {
        /// <summary>
        /// Converts <see cref="UserNotificationInfoWithNotificationInfo"/> to <see cref="UserNotification"/>.
        /// 转换<see cref="UserNotificationInfoWithNotificationInfo"/> 到 <see cref="UserNotification"/>
        /// </summary>
        public static UserNotification ToUserNotification(this UserNotificationInfoWithNotificationInfo userNotificationInfoWithNotificationInfo)
        {
            return userNotificationInfoWithNotificationInfo.UserNotification.ToUserNotification(
                userNotificationInfoWithNotificationInfo.Notification.ToTenantNotification()
                );
        }
    }
}