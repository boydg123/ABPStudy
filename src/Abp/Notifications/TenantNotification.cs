using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Abp.Notifications
{
    /// <summary>
    /// Represents a published notification for a tenant/user.
    /// 标识租户/用户的一个发布通知
    /// </summary>
    [Serializable]
    public class TenantNotification : EntityDto<Guid>, IHasCreationTime
    {
        /// <summary>
        /// Tenant Id.
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// Unique notification name.
        /// 通知唯一名称
        /// </summary>
        public string NotificationName { get; set; }

        /// <summary>
        /// Notification data.
        /// 通知数据
        /// </summary>
        public NotificationData Data { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// 获取或设置实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Name of the entity type (including namespaces).
        /// 实体名称(包含命名空间)
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// Entity id.
        /// 实体ID
        /// </summary>
        public object EntityId { get; set; }

        /// <summary>
        /// Severity.
        /// 通知严重程度
        /// </summary>
        public NotificationSeverity Severity { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantNotification"/> class.
        /// 初始化<see cref="TenantNotification"/>类新的实例
        /// </summary>
        public TenantNotification()
        {
            CreationTime = Clock.Now;
        }
    }
}