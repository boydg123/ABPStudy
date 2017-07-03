using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Localization
{
    /// <summary>
    /// 用于存储一个本地化文本
    /// </summary>
    [Serializable]
    [Table("AbpLanguageTexts")]
    public class ApplicationLanguageText : AuditedEntity<long>, IMayHaveTenant
    {
        public const int MaxSourceNameLength = 128;
        public const int MaxKeyLength = 256;
        public const int MaxValueLength = 64 * 1024 * 1024; //64KB

        /// <summary>
        /// TenantId of this entity. Can be null for host.
        /// 商户ID，如果是宿主商户则可以为Null
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Language name (culture name). Matches to <see cref="ApplicationLanguage.Name"/>.
        /// 语言名称(区域名)，匹配自<see cref="ApplicationLanguage.Name"/>.
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string LanguageName { get; set; }

        /// <summary>
        /// 本地化源名称
        /// </summary>
        [Required]
        [StringLength(MaxSourceNameLength)]
        public virtual string Source { get; set; }

        /// <summary>
        /// 本地化Key
        /// </summary>
        [Required]
        [StringLength(MaxKeyLength)]
        public virtual string Key { get; set; }

        /// <summary>
        /// 本地化值
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(MaxValueLength)]
        public virtual string Value { get; set; }
    }
}