using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Localization;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 设置默认语言Input
    /// </summary>
    public class SetDefaultLanguageInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}