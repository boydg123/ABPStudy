using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 应用程序语言列表Dto
    /// </summary>
    [AutoMapFrom(typeof(ApplicationLanguage))]
    public class ApplicationLanguageListDto : FullAuditedEntityDto
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }
    }
}