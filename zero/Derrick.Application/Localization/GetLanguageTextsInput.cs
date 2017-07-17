using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Validation;

namespace Derrick.Localization
{
    /// <summary>
    /// 获取语言文本Input
    /// </summary>
    public class GetLanguageTextsInput :IPagedResultRequest, ISortedResultRequest, IShouldNormalize
    {
        /// <summary>
        /// 最大结果数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public int MaxResultCount { get; set; } //0: Unlimited.
        /// <summary>
        /// 跳过数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }
        /// <summary>
        /// 排序规则
        /// </summary>
        public string Sorting { get; set; }
        /// <summary>
        /// 源名称
        /// </summary>
        [Required]
        [MaxLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }
        /// <summary>
        /// 基本语言名称
        /// </summary>
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string BaseLanguageName { get; set; }
        /// <summary>
        /// 目标语言名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength, MinimumLength = 2)]
        public string TargetLanguageName { get; set; }
        /// <summary>
        /// 目标值过滤条件
        /// </summary>
        public string TargetValueFilter { get; set; }
        /// <summary>
        /// 过滤文本
        /// </summary>
        public string FilterText { get; set; }
        /// <summary>
        /// 标准化
        /// </summary>
        public void Normalize()
        {
            if (TargetValueFilter.IsNullOrEmpty())
            {
                TargetValueFilter = "ALL";
            }
        }
    }
}