using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 语言编辑Output
    /// </summary>
    public class GetLanguageForEditOutput
    {
        /// <summary>
        /// 语言编辑Dto
        /// </summary>
        public ApplicationLanguageEditDto Language { get; set; }
        /// <summary>
        /// 语言名称集合
        /// </summary>
        public List<ComboboxItemDto> LanguageNames { get; set; }
        /// <summary>
        /// 标记集合
        /// </summary>
        public List<ComboboxItemDto> Flags { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetLanguageForEditOutput()
        {
            LanguageNames = new List<ComboboxItemDto>();
            Flags = new List<ComboboxItemDto>();
        }
    }
}