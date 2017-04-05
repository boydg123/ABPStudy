using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Threading;

namespace Abp.Notifications
{
    /// <summary>
    /// Extension methods for
    /// <see cref="INotificationSubscriptionManager"/>, 
    /// <see cref="INotificationPublisher"/>, 
    /// <see cref="IUserNotificationManager"/>.
    /// 的扩展方法
    /// </summary>
    public static class NotificationExtensions
    {
        #region INotificationSubscriptionManager 通知订阅管理器

        /// <summary>
        /// Subscribes to a notification. 
        /// 订阅一个通知
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        public static void Subscribe(this INotificationSubscriptionManager notificationSubscriptionManager, UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            AsyncHelper.RunSync(() => notificationSubscriptionManager.SubscribeAsync(user, notificationName, entityIdentifier));
        }

        /// <summary>
        /// Subscribes to all available notifications for given user.It does not subscribe entity related notifications.
        /// 为给定用户订阅所有可用通知，它不订阅通知相关的实体
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="user">User. / 用户</param>
        public static void SubscribeToAllAvailableNotifications(this INotificationSubscriptionManager notificationSubscriptionManager, UserIdentifier user)
        {
            AsyncHelper.RunSync(() => notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user));            
        }

        /// <summary>
        /// Unsubscribes from a notification.
        /// 从通知退订
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        public static void Unsubscribe(this INotificationSubscriptionManager notificationSubscriptionManager, UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            AsyncHelper.RunSync(() => notificationSubscriptionManager.UnsubscribeAsync(user, notificationName, entityIdentifier));
        }

        /// <summary>
        /// Gets all subscribtions for given notification.TODO: Can work only for single database approach!
        /// 为给定通知获取所有订阅。TODO:仅仅在单个数据库方法工作
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        public static List<NotificationSubscription> GetSubscriptions(this INotificationSubscriptionManager notificationSubscriptionManager, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            return AsyncHelper.RunSync(() => notificationSubscriptionManager.GetSubscriptionsAsync(notificationName, entityIdentifier));
        }

        /// <summary>
        /// Gets all subscribtions for given notification.
        /// 为给定通知获取所有订阅
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="tenantId">Tenant id. Null for the host. / 租户ID，宿主则为null</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        public static List<NotificationSubscription> GetSubscriptions(this INotificationSubscriptionManager notificationSubscriptionManager, int? tenantId, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            return AsyncHelper.RunSync(() => notificationSubscriptionManager.GetSubscriptionsAsync(tenantId, notificationName, entityIdentifier));
        }

        /// <summary>
        /// Gets subscribed notifications for a user.
        /// 为给定用户获取订阅通知
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="user">User. / 用户</param>
        public static List<NotificationSubscription> GetSubscribedNotifications(this INotificationSubscriptionManager notificationSubscriptionManager, UserIdentifier user)
        {
            return AsyncHelper.RunSync(() => notificationSubscriptionManager.GetSubscribedNotificationsAsync(user));
        }

        /// <summary>
        /// Checks if a user subscribed for a notification.
        /// 检查用户是否订阅了通知
        /// </summary>
        /// <param name="notificationSubscriptionManager">Notification subscription manager / 通知订阅管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        public static bool IsSubscribed(this INotificationSubscriptionManager notificationSubscriptionManager, UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            return AsyncHelper.RunSync(() => notificationSubscriptionManager.IsSubscribedAsync(user, notificationName, entityIdentifier));
        }

        #endregion

        #region INotificationPublisher 通知发布者

        /// <summary>
        /// Publishes a new notification.
        /// 发布一个通知
        /// </summary>
        /// <param name="notificationPublisher">Notification publisher / 通知发布者</param>
        /// <param name="notificationName">Unique notification name / 通知唯一名称</param>
        /// <param name="data">Notification data (optional) / 通知数据(可选)</param>
        /// <param name="entityIdentifier">The entity identifier if this notification is related to an entity / 实体标识，如果通知关联了一个实体</param>
        /// <param name="severity">Notification severity / 通知严重程度</param>
        /// <param name="userIds">Target user id(s). Used to send notification to specific user(s). If this is null/empty, the notification is sent to all subscribed users / 租户ID，用户发送通知给特定用户，如果为 null/空，通知发送给所有的订阅用户</param>
        public static void Publish(this INotificationPublisher notificationPublisher, string notificationName, NotificationData data = null, EntityIdentifier entityIdentifier = null, NotificationSeverity severity = NotificationSeverity.Info, UserIdentifier[] userIds = null)
        {
            AsyncHelper.RunSync(() => notificationPublisher.PublishAsync(notificationName, data, entityIdentifier, severity, userIds));
        }

        #endregion

        #region IUserNotificationManager 用户通知管理器

        /// <summary>
        /// Gets notifications for a user.
        /// 为指定用户获取通知
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">State / 状态</param>
        /// <param name="skipCount">Skip count. / 跳过数量</param>
        /// <param name="maxResultCount">Maximum result count. / 最大返回数量</param>
        public static List<UserNotification> GetUserNotifications(this IUserNotificationManager userNotificationManager, UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return AsyncHelper.RunSync(() => userNotificationManager.GetUserNotificationsAsync(user, state, skipCount: skipCount, maxResultCount: maxResultCount));
        }

        /// <summary>
        /// Gets user notification count.
        /// 获取用户通知数量
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">State. / 状态</param>
        public static int GetUserNotificationCount(this IUserNotificationManager userNotificationManager, UserIdentifier user, UserNotificationState? state = null)
        {
            return AsyncHelper.RunSync(() => userNotificationManager.GetUserNotificationCountAsync(user, state));
        }

        /// <summary>
        /// Gets a user notification by given id.
        /// 为指定ID获取一个用户通知
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="tenantId">Tenant Id / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户通知ID</param>
        public static UserNotification GetUserNotification(this IUserNotificationManager userNotificationManager, int? tenantId, Guid userNotificationId)
        {
            return AsyncHelper.RunSync(() => userNotificationManager.GetUserNotificationAsync(tenantId, userNotificationId));
        }

        /// <summary>
        /// Updates a user notification state.
        /// 更新一个用户通知状态
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="tenantId">Tenant Id / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户通知ID</param>
        /// <param name="state">New state. / 状态</param>
        public static void UpdateUserNotificationState(this IUserNotificationManager userNotificationManager, int? tenantId, Guid userNotificationId, UserNotificationState state)
        {
            AsyncHelper.RunSync(() => userNotificationManager.UpdateUserNotificationStateAsync(tenantId, userNotificationId, state));
        }

        /// <summary>
        /// Updates all notification states for a user.
        /// 为指定用户更新所有通知状态
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">New state. / 状态</param>
        public static void UpdateAllUserNotificationStates(this IUserNotificationManager userNotificationManager, UserIdentifier user, UserNotificationState state)
        {
            AsyncHelper.RunSync(() => userNotificationManager.UpdateAllUserNotificationStatesAsync(user, state));
        }

        /// <summary>
        /// Deletes a user notification.
        /// 删除一个用户通知
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="tenantId">Tenant Id / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户标识ID</param>
        public static void DeleteUserNotification(this IUserNotificationManager userNotificationManager, int? tenantId, Guid userNotificationId)
        {
            AsyncHelper.RunSync(() => userNotificationManager.DeleteUserNotificationAsync(tenantId, userNotificationId));
        }

        /// <summary>
        /// Deletes all notifications of a user.
        /// 为指定用户删除所有通知
        /// </summary>
        /// <param name="userNotificationManager">User notificaiton manager / 用户通知管理器</param>
        /// <param name="user">The user id. / 用户ID</param>
        public static void DeleteAllUserNotifications(this IUserNotificationManager userNotificationManager, UserIdentifier user)
        {
            AsyncHelper.RunSync(() => userNotificationManager.DeleteAllUserNotificationsAsync(user));
        }

        #endregion
    }
}