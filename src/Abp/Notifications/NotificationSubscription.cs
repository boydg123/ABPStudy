using System;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Abp.Notifications
{
    /// <summary>
    /// Represents a user subscription to a notification.
    /// 表示用户订阅通知
    /// </summary>
    public class NotificationSubscription : IHasCreationTime
    {
        /// <summary>
        /// Tenant id of the subscribed user.
        /// 订阅用户的租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Notification unique name.
        /// 通知唯一名称
        /// </summary>
        public string NotificationName { get; set; }

        /// <summary>
        /// Entity type.
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Name of the entity type (including namespaces).
        /// 实体类型的名称(包含命名空间)
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// Entity Id.
        /// 实体ID
        /// </summary>
        public object EntityId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSubscription"/> class.
        /// 初始化<see cref="NotificationSubscription"/>类新的实例
        /// </summary>
        public NotificationSubscription()
        {
            CreationTime = Clock.Now;
        }
    }
}