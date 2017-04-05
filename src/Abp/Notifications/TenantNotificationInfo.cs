using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Notifications
{
    /// <summary>
    /// A notification distributed to it's related tenant.
    /// 分发给其相关租户的通知
    /// </summary>
    [Table("AbpTenantNotifications")]
    public class TenantNotificationInfo : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// Tenant id of the subscribed user.
        /// 订阅用户的租户ID
        /// 
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Unique notification name.
        /// 通知的唯一名称
        /// </summary>
        [Required]
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public virtual string NotificationName { get; set; }

        /// <summary>
        /// Notification data as JSON string.
        /// 通知JSON字符串数据
        /// </summary>
        [MaxLength(NotificationInfo.MaxDataLength)]
        public virtual string Data { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="Data"/>.It's AssemblyQualifiedName of the type.
        /// JSON序列化<see cref="Data"/>的类型，它将是类型的程序集名称
        /// </summary>
        [MaxLength(NotificationInfo.MaxDataTypeNameLength)]
        public virtual string DataTypeName { get; set; }

        /// <summary>
        /// Gets/sets entity type name, if this is an entity level notification.It's FullName of the entity type.
        /// 获取/设置实体类型名称，如果这是一个实体级别的通知，它将是实体类型的全名称
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
        /// Notification severity.
        /// 通知严重程度
        /// </summary>
        public virtual NotificationSeverity Severity { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TenantNotificationInfo()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="notification">通知</param>
        public TenantNotificationInfo(int? tenantId, NotificationInfo notification)
        {
            TenantId = tenantId;
            NotificationName = notification.NotificationName;
            Data = notification.Data;
            DataTypeName = notification.DataTypeName;
            EntityTypeName = notification.EntityTypeName;
            EntityTypeAssemblyQualifiedName = notification.EntityTypeAssemblyQualifiedName;
            EntityId = notification.EntityId;
            Severity = notification.Severity;
        }
    }
}