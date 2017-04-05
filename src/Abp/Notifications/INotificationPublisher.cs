using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Runtime.Session;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to publish notifications.
    /// 用于发布通知
    /// </summary>
    public interface INotificationPublisher
    {
        /// <summary>
        /// Publishes a new notification.
        /// 发布一个新通知
        /// </summary>
        /// <param name="notificationName">Unique notification name / 唯一的通知名称</param>
        /// <param name="data">Notification data (optional) / 通知数据(可选)</param>
        /// <param name="entityIdentifier">The entity identifier if this notification is related to an entity / 标识实体(如果通知关联了一个实体)</param>
        /// <param name="severity">Notification severity / 通知严重程度</param>
        /// <param name="userIds">
        /// Target user id(s). Used to send notification to specific user(s) (without checking the subscription). 
        /// 目标用户ID,用于发送通知给明确的用户(没有检查订阅)
        /// If this is null/empty, the notification is sent to subscribed users.
        /// 如果是null/空，通知发送给订阅用户
        /// </param>
        /// <param name="excludedUserIds">
        /// Excluded user id(s).This can be set to exclude some users while publishing notifications to subscribed users.
        /// 排除用户ID，当向订阅用户发布通知时，可以设置为排除某些用户
        /// It's normally not set if <see cref="userIds"/> is set.
        /// 如果<see cref="userIds"/>设置了则它通常不设置
        /// </param>
        /// <param name="tenantIds">
        /// Target tenant id(s).Used to send notification to subscribed users of specific tenant(s).
        /// 目标租户ID，用于向特定租户发送订阅用户的通知
        /// This should not be set if <see cref="userIds"/> is set.
        /// 如果<see cref="userIds"/>有值，则这个不应该被设置
        /// <see cref="NotificationPublisher.AllTenants"/> can be passed to indicate all tenants.
        /// <see cref="NotificationPublisher.AllTenants"/>可以通过指示所有租户
        /// But this can only work in a single database approach (all tenants are stored in host database).
        /// 但是这个方法仅仅用于多租户应用单一数据库的方法(所有租户存储在一个数据库)
        /// If this is null, then it's automatically set to the current tenant on <see cref="IAbpSession.TenantId"/>.
        /// 当这个为null时，它将自动设置为当前租户<see cref="IAbpSession.TenantId"/>
        /// </param>
        Task PublishAsync(
            string notificationName,
            NotificationData data = null,
            EntityIdentifier entityIdentifier = null,
            NotificationSeverity severity = NotificationSeverity.Info,
            UserIdentifier[] userIds = null,
            UserIdentifier[] excludedUserIds = null,
            int?[] tenantIds = null);
    }
}