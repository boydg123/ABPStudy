using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Configuration
{
    /// <summary>
    /// Represents a setting for a tenant or user.
    /// 表示商户或用户的设置
    /// </summary>
    [Table("AbpSettings")]
    public class Setting : AuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="Name"/>属性的最大长度
        /// </summary>
        public const int MaxNameLength = 256;

        /// <summary>
        /// <see cref="Value"/>属性的最大长度
        /// </summary>
        public const int MaxValueLength = 2000;

        /// <summary>
        /// TenantId for this setting.TenantId is null if this setting is not Tenant level.
        /// 商户ID，如果设置不是商户级别的设置则商户ID为Null
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// UserId for this setting.UserId is null if this setting is not user level.
        /// 用户ID，如果设置不是用户级别的设置则用户ID为Null
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 设置的唯一名称
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// 设置的值
        /// </summary>
        [MaxLength(MaxValueLength)]
        public virtual string Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Setting()
        {

        }

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">TenantId for this setting / 此设置的商户ID</param>
        /// <param name="userId">UserId for this setting / 此设置的用户ID</param>
        /// <param name="name">Unique name of the setting / 此设置的唯一名称</param>
        /// <param name="value">Value of the setting / 此设置的值</param>
        public Setting(int? tenantId, long? userId, string name, string value)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = name;
            Value = value;
        }
    }
}