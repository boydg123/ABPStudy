using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store (persist) notifications.
    /// 用于存储(持续)通知
    /// </summary>
    public interface INotificationStore
    {
        /// <summary>
        /// Inserts a notification subscription.
        /// 插入一个订阅通知 - 异步
        /// </summary>
        Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription);

        /// <summary>
        /// Deletes a notification subscription.
        /// 删除一个订阅通知 - 异步
        /// </summary>
        Task DeleteSubscriptionAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Inserts a notification.
        /// 插入一个通知 - 异步
        /// </summary>
        Task InsertNotificationAsync(NotificationInfo notification);

        /// <summary>
        /// Gets a notification by Id, or returns null if not found.
        /// 通过ID获取通知，如果没有找到则返回null
        /// </summary>
        Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId);

        /// <summary>
        /// Inserts a user notification.
        /// 插入一个用户通知
        /// </summary>
        Task InsertUserNotificationAsync(UserNotificationInfo userNotification);

        /// <summary>
        /// Gets subscriptions for a notification.
        /// 获取通知订阅
        /// </summary>
        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a notification for specified tenant(s).
        /// 为指定的用户获取通知订阅
        /// </summary>
        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(int?[] tenantIds, string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a user.
        /// 获取用户的订阅
        /// </summary>
        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(UserIdentifier user);

        /// <summary>
        /// Checks if a user subscribed for a notification
        /// 检查用户是否订阅了通知
        /// </summary>
        Task<bool> IsSubscribedAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Updates a user notification state.
        /// 更新用户通知状态
        /// </summary>
        Task UpdateUserNotificationStateAsync(int? notificationId, Guid userNotificationId, UserNotificationState state);

        /// <summary>
        /// Updates all notification states for a user.
        /// 更新用户的所有通知状态
        /// </summary>
        Task UpdateAllUserNotificationStatesAsync(UserIdentifier user, UserNotificationState state);

        /// <summary>
        /// Deletes a user notification.
        /// 删除用户通知
        /// </summary>
        Task DeleteUserNotificationAsync(int? notificationId, Guid userNotificationId);

        /// <summary>
        /// Deletes all notifications of a user.
        /// 删除所有用户的通知
        /// </summary>
        Task DeleteAllUserNotificationsAsync(UserIdentifier user);

        /// <summary>
        /// Gets notifications of a user.
        /// 被通知的用户
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="skipCount">Skip count. / 跳过计数</param>
        /// <param name="maxResultCount">Maximum result count. / 最大结果数</param>
        /// <param name="state">State / 状态</param>
        Task<List<UserNotificationInfoWithNotificationInfo>> GetUserNotificationsWithNotificationsAsync(UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue);

        /// <summary>
        /// Gets user notification count.
        /// 获取用户通知的数量
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">The state. / 状态</param>
        Task<int> GetUserNotificationCountAsync(UserIdentifier user, UserNotificationState? state = null);

        /// <summary>
        /// Gets a user notification.
        /// 获取用户的通知
        /// </summary>
        /// <param name="tenantId">Tenant Id / 租户ID</param>
        /// <param name="userNotificationId">Skip count. / 跳过的数量</param>
        Task<UserNotificationInfoWithNotificationInfo> GetUserNotificationWithNotificationOrNullAsync(int? tenantId, Guid userNotificationId);

        /// <summary>
        /// Inserts notification for a tenant.
        /// 为租户插入通知
        /// </summary>
        Task InsertTenantNotificationAsync(TenantNotificationInfo tenantNotificationInfo);

        /// <summary>
        /// Deletes the notification.
        /// 删除通知
        /// </summary>
        /// <param name="notification">The notification. / 通知实体</param>
        Task DeleteNotificationAsync(NotificationInfo notification);
    }
}