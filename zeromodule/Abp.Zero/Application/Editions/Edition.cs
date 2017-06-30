using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Abp.Application.Editions
{
    /// <summary>
    /// Represents an edition of the application.
    /// 表示应用程序的版本
    /// </summary>
    [Table("AbpEditions")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class Edition : FullAuditedEntity
    {
        /// <summary>
        /// <see cref="Name"/>属性的最大长度
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// <see cref="DisplayName"/>属性的最大长度 
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// Unique name of this edition.
        /// 此版本的唯一名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this edition.
        /// 此版本的显示名称
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Edition()
        {
            Name = Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="displayName"></param>
        public Edition(string displayName)
            : this()
        {
            DisplayName = displayName;
        }
    }
}