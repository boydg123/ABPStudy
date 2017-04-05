using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Json;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store a notification subscription.
    /// 用于存储通知订阅
    /// </summary>
    [Table("AbpNotificationSubscriptions")]
    public class NotificationSubscriptionInfo : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// Tenant id of the subscribed user.
        /// 订阅用户的租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Notification unique name.
        /// 通知的唯一名称
        /// </summary>
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public virtual string NotificationName { get; set; }

        /// <summary>
        /// Gets/sets entity type name, if this is an entity level notification.It's FullName of the entity type.
        /// 获取/设置实体类型名称，如果这是一个实体级别的通知，则是实体类型的全名称
        /// </summary>
        [MaxLength(NotificationInfo.MaxEntityTypeNameLength)]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// AssemblyQualifiedName of the entity type.
        /// 实体类型的程序集名称
        /// </summary>
        [MaxLength(NotificationInfo.MaxEntityTypeAssemblyQualifiedNameLength)]
        public virtual string EntityTypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity, if this is an entity level notification.
        /// 获取/设置实体的主键，如果这是一个实体级别的通知
        /// </summary>
        [MaxLength(NotificationInfo.MaxEntityIdLength)]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSubscriptionInfo"/> class.
        /// 初始化<see cref="NotificationSubscriptionInfo"/>类一个新的实例
        /// </summary>
        public NotificationSubscriptionInfo()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSubscriptionInfo"/> class.
        /// 初始化<see cref="NotificationSubscriptionInfo"/>类一个新的实例
        /// </summary>
        public NotificationSubscriptionInfo(int? tenantId, long userId, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            TenantId = tenantId;
            NotificationName = notificationName;
            UserId = userId;
            EntityTypeName = entityIdentifier == null ? null : entityIdentifier.Type.FullName;
            EntityTypeAssemblyQualifiedName = entityIdentifier == null ? null : entityIdentifier.Type.AssemblyQualifiedName;
            EntityId = entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString();
        }
    }
}