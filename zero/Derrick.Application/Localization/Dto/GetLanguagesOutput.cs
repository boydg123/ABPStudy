using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 获取语言Output
    /// </summary>
    public class GetLanguagesOutput : ListResultDto<ApplicationLanguageListDto>
    {
        /// <summary>
        /// 默认语言名称
        /// </summary>
        public string DefaultLanguageName { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetLanguagesOutput()
        {
            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">应用程序语言列表Dto</param>
        /// <param name="defaultLanguageName">默认语言名称</param>
        public GetLanguagesOutput(IReadOnlyList<ApplicationLanguageListDto> items, string defaultLanguageName)
            : base(items)
        {
            DefaultLanguageName = defaultLanguageName;
        }
    }
}