using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Localization
{
    /// <summary>
    /// 表示应用程序的语言
    /// </summary>
    [Serializable]
    [Table("AbpLanguages")]
    public class ApplicationLanguage : FullAuditedEntity, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="Name"/>属性的最大长度
        /// </summary>
        public const int MaxNameLength = 10;

        /// <summary>
        /// <see cref="DisplayName"/>属性的最大长度
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// <see cref="Icon"/>属性的最大长度
        /// </summary>
        public const int MaxIconLength = 128;

        /// <summary>
        /// TenantId of this entity. Can be null for host.
        /// 商户ID，如果是宿主商户则为Null
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture, like "en" or "en-US".
        /// 区域名称."cn" 或 "zh-cn"
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// ICON
        /// </summary>
        [StringLength(MaxIconLength)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationLanguage()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="name">区域名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="icon">ICON</param>
        public ApplicationLanguage(int? tenantId, string name, string displayName, string icon = null)
        {
            TenantId = tenantId;
            Name = name;
            DisplayName = displayName;
            Icon = icon;
        }
        /// <summary>
        /// 转换成语言信息对象
        /// </summary>
        /// <returns></returns>
        public virtual LanguageInfo ToLanguageInfo()
        {
            return new LanguageInfo(Name, DisplayName, Icon);
        }
    }
}
