namespace Abp.Notifications
{
    /// <summary>
    /// Extension methods for <see cref="UserNotificationInfo"/>.
    /// <see cref="UserNotificationInfo"/>的扩展方法
    /// </summary>
    public static class UserNotificationInfoExtensions
    {
        /// <summary>
        /// Converts <see cref="UserNotificationInfo"/> to <see cref="UserNotification"/>.
        /// 转换<see cref="UserNotificationInfo"/> 到 <see cref="UserNotification"/>.
        /// </summary>
        public static UserNotification ToUserNotification(this UserNotificationInfo userNotificationInfo, TenantNotification tenantNotification)
        {
            return new UserNotification
            {
                Id = userNotificationInfo.Id,
                Notification = tenantNotification,
                UserId = userNotificationInfo.UserId,
                State = userNotificationInfo.State,
                TenantId = userNotificationInfo.TenantId
            };
        }
    }
}