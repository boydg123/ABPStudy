using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Localization;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 更新语言文本Input
    /// </summary>
    public class UpdateLanguageTextInput
    {
        /// <summary>
        /// 语言名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string LanguageName { get; set; }
        /// <summary>
        /// 源名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguageText.MaxKeyLength)]
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(ApplicationLanguageText.MaxValueLength)]
        public string Value { get; set; }
    }
}