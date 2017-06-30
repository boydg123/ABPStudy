using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Abp.Application.Features
{
    /// <summary>
    /// 设置的基类
    /// </summary>
    [Table("AbpFeatures")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class FeatureSetting : CreationAuditedEntity<long>
    {
        /// <summary>
        /// <see cref="Name"/>字段的最大长度
        /// </summary>
        public const int MaxNameLength = 128;

        /// <summary>
        /// <see cref="Value"/>属性的最大长度
        /// </summary>
        public const int MaxValueLength = 2000;

        /// <summary>
        /// 功能名称
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 功能值
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [MaxLength(MaxValueLength)]
        public virtual string Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected FeatureSetting()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">功能名称</param>
        /// <param name="value">功能值</param>
        protected FeatureSetting(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}