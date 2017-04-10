using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP用户本地化配置Dto
    /// </summary>
    public class AbpUserLocalizationConfigDto
    {
        public AbpUserCurrentCultureConfigDto CurrentCulture { get; set; }

        public List<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public List<AbpLocalizationSourceDto> Sources { get; set; }

        public Dictionary<string, Dictionary<string, string>> Values { get; set; }
    }
}