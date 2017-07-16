using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Abp.Notifications
{
    /// <summary>
    /// 用于管理通知的订阅
    /// </summary>
    public interface INotificationSubscriptionManager
    {
        /// <summary>
        /// 为指定用户和通知消息订阅一个通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">标识实体</param>
        Task SubscribeAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// 为指定用户订阅所有可用的通知，它不订阅通知相关的实体
        /// </summary>
        /// <param name="user">用户标识.</param>
        Task SubscribeToAllAvailableNotificationsAsync(UserIdentifier user);

        /// <summary>
        /// 退订一个通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">标识实体</param>
        Task UnsubscribeAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// Gets all subscribtions for given notification (including all tenants).
        /// 为给定的通知获取所有订阅(包含所有租户)
        /// This only works for single database approach in a multitenant application!
        /// 这个方法仅仅用于多租户应用单一数据库的方法
        /// </summary>
        /// <param name="notificationName">Name of the notification . / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        Task<List<NotificationSubscription>> GetSubscriptionsAsync(string notificationName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// Gets all subscribtions for given notification.
        /// 为指定的通知获取所有订阅
        /// </summary>
        /// <param name="tenantId">Tenant id. Null for the host. / 租户ID，宿主则是null</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        Task<List<NotificationSubscription>> GetSubscriptionsAsync(int? tenantId, string notificationName, EntityIdentifier entityIdentifier = null);

        /// <summary>
        /// Gets subscribed notifications for a user.
        /// 为用户获取所有订阅通知
        /// </summary>
        /// <param name="user">User. / 用户</param>
        Task<List<NotificationSubscription>> GetSubscribedNotificationsAsync(UserIdentifier user);

        /// <summary>
        /// Checks if a user subscribed for a notification.
        /// 检查用户是否订阅了通知
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="notificationName">Name of the notification. / 通知名称</param>
        /// <param name="entityIdentifier">entity identifier / 标识实体</param>
        Task<bool> IsSubscribedAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null);
    }
}
