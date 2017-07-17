using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Localization;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 应用程序语言编辑Dto
    /// </summary>
    [AutoMapFrom(typeof(ApplicationLanguage))]
    public class ApplicationLanguageEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int? Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(ApplicationLanguage.MaxIconLength)]
        public virtual string Icon { get; set; }
    }
}