using Abp.Application.Services.Dto;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 语言文本列表Dto
    /// </summary>
    public class LanguageTextListDto
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 基础值
        /// </summary>
        public string BaseValue { get; set; }
        /// <summary>
        /// 目标值
        /// </summary>
        public string TargetValue { get; set; }
    }
}