using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store a notification request.
    /// 用于存储一个通知请求
    /// This notification is distributed to tenants and users by <see cref="INotificationDistributer"/>.
    /// 通过<see cref="INotificationDistributer"/>将此通知分发给租户和用户
    /// </summary>
    [Serializable]
    [Table("AbpNotifications")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class NotificationInfo : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// Indicates all tenant ids for <see cref="TenantIds"/> property.
        /// <see cref="TenantIds"/>属性指示所有租户IDS
        /// Value: "0".
        /// </summary>
        public const string AllTenantIds = "0";

        /// <summary>
        /// Maximum length of <see cref="NotificationName"/> property.
        /// <see cref="NotificationName"/>属性的最大长度
        /// Value: 96.
        /// </summary>
        public const int MaxNotificationNameLength = 96;

        /// <summary>
        /// Maximum length of <see cref="Data"/> property.
        /// <see cref="Data"/>属性的最大长度
        /// Value: 1048576 (1 MB).
        /// </summary>
        public const int MaxDataLength = 1024 * 1024;

        /// <summary>
        /// Maximum length of <see cref="DataTypeName"/> property.
        /// <see cref="DataTypeName"/>属性的最大长度
        /// Value: 512.
        /// </summary>
        public const int MaxDataTypeNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityTypeName"/> property.
        /// <see cref="EntityTypeName"/>属性的最大长度
        /// Value: 250.
        /// </summary>
        public const int MaxEntityTypeNameLength = 250;

        /// <summary>
        /// Maximum length of <see cref="EntityTypeAssemblyQualifiedName"/> property.
        /// <see cref="EntityTypeAssemblyQualifiedName"/>属性的最大长度
        /// Value: 512.
        /// </summary>
        public const int MaxEntityTypeAssemblyQualifiedNameLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityId"/> property.
        /// <see cref="EntityId"/>属性的最大长度
        /// Value: 96.
        /// </summary>
        public const int MaxEntityIdLength = 96;

        /// <summary>
        /// Maximum length of <see cref="UserIds"/> property.
        /// <see cref="UserIds"/>属性的最大长度
        /// Value: 131072 (128 KB).
        /// </summary>
        public const int MaxUserIdsLength = 128 * 1024;

        /// <summary>
        /// Maximum length of <see cref="TenantIds"/> property.
        /// <see cref="TenantIds"/>属性的最大长度
        /// Value: 131072 (128 KB).
        /// </summary>
        public const int MaxTenantIdsLength = 128 * 1024;

        /// <summary>
        /// Unique notification name.
        /// 通知唯一名称
        /// </summary>
        [Required]
        [MaxLength(MaxNotificationNameLength)]
        public virtual string NotificationName { get; set; }

        /// <summary>
        /// Notification data as JSON string.
        /// 通知数据JSON
        /// </summary>
        [MaxLength(MaxDataLength)]
        public virtual string Data { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="Data"/>.It's AssemblyQualifiedName of the type.
        /// <see cref="Data"/>的JSON序列化类型，类型的程序集名称
        /// </summary>
        [MaxLength(MaxDataTypeNameLength)]
        public virtual string DataTypeName { get; set; }

        /// <summary>
        /// Gets/sets entity type name, if this is an entity level notification.It's FullName of the entity type.
        /// 获取/设置实体类型名称，如果这是一个实体级别的通知，则是实体类型的全名称
        /// </summary>
        [MaxLength(MaxEntityTypeNameLength)]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// AssemblyQualifiedName of the entity type.
        /// 实体类别的程序集名称
        /// </summary>
        [MaxLength(MaxEntityTypeAssemblyQualifiedNameLength)]
        public virtual string EntityTypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity, if this is an entity level notification.
        /// 获取/设置实体的主键，如果这是一个实体级别的通知
        /// </summary>
        [MaxLength(MaxEntityIdLength)]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// Notification severity.
        /// 通知严重程度
        /// </summary>
        public virtual NotificationSeverity Severity { get; set; }

        /// <summary>
        /// Target users of the notification.If this is set, it overrides subscribed users.
        /// 通知的目标用户，如果这有值，它将覆盖订阅用户
        /// If this is null/empty, then notification is sent to all subscribed users.
        /// 如果为null/空，将通知所有的订阅用户
        /// </summary>
        [MaxLength(MaxUserIdsLength)]
        public virtual string UserIds { get; set; }

        /// <summary>
        /// Excluded users.This can be set to exclude some users while publishing notifications to subscribed users.
        /// 排除用户,当发布通知给订阅用户的时候用来排除某些用户
        /// It's not normally used if <see cref="UserIds"/> is not null.
        /// 如果<see cref="UserIds"/>不为null的时候它通常不被使用
        /// </summary>
        [MaxLength(MaxUserIdsLength)]
        public virtual string ExcludedUserIds { get; set; }

        /// <summary>
        /// Target tenants of the notification.Used to send notification to subscribed users of specific tenant(s).
        /// 通知的目标租户，用来发送通知给那些租户的订阅用户
        /// This is valid only if UserIds is null.If it's "0", then indicates to all tenants.
        /// 如果用户id为null则这是有效的，如果它是"0",则指示所有租户
        /// </summary>
        [MaxLength(MaxTenantIdsLength)]
        public virtual string TenantIds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationInfo"/> class.
        /// 初始化<see cref="NotificationInfo"/>类新的实例
        /// </summary>
        public NotificationInfo()
        {
            Severity = NotificationSeverity.Info;
        }
    }
}