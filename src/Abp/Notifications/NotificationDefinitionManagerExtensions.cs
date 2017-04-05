using System.Collections.Generic;
using Abp.Threading;

namespace Abp.Notifications
{
    /// <summary>
    /// Extension methods for <see cref="INotificationDefinitionManager"/>.
    /// <see cref="INotificationDefinitionManager"/>的扩展方法
    /// </summary>
    public static class NotificationDefinitionManagerExtensions
    {
        /// <summary>
        /// Gets all available notification definitions for given user.
        /// 为指定的用户获取所有可用的通知定义
        /// </summary>
        /// <param name="notificationDefinitionManager">Notification definition manager / 通知定义管理器</param>
        /// <param name="user">User</param>
        public static IReadOnlyList<NotificationDefinition> GetAllAvailable(this INotificationDefinitionManager notificationDefinitionManager, UserIdentifier user)
        {
            return AsyncHelper.RunSync(() => notificationDefinitionManager.GetAllAvailableAsync(user));
        }
    }
}