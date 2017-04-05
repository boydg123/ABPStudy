using System;

namespace Abp.Notifications
{
    /// <summary>
    /// Represents state of a <see cref="UserNotification"/>.
    /// 标识<see cref="UserNotification"/>的状态
    /// </summary>
    [Serializable]
    public enum UserNotificationState
    {
        /// <summary>
        /// Notification is not read by user yet.
        /// 通知还没被用户读取
        /// </summary>
        Unread = 0,

        /// <summary>
        /// Notification is read by user.
        /// 通知已被用户读取
        /// </summary>
        Read
    }
}