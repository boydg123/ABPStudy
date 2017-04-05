using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store a user notification.
    /// 用来存储一个用户通知
    /// </summary>
    [Serializable]
    [Table("AbpUserNotifications")]
    public class UserNotificationInfo : Entity<Guid>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// Tenant Id.
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Notification Id.
        /// 通知ID
        /// </summary>
        [Required]
        public virtual Guid TenantNotificationId { get; set; }

        /// <summary>
        /// Current state of the user notification.
        /// 用户通知的当前通知
        /// </summary>
        public virtual UserNotificationState State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationInfo"/> class.
        /// 初始化<see cref="UserNotificationInfo"/> 类新的实例
        /// </summary>
        public UserNotificationInfo()
        {
            State = UserNotificationState.Unread;
            CreationTime = Clock.Now;
        }
    }
}