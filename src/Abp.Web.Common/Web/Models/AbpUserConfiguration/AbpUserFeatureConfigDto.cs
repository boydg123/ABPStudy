using System.Collections.Generic;

namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP用户功能配置Dto
    /// </summary>
    public class AbpUserFeatureConfigDto
    {
        public Dictionary<string, AbpStringValueDto> AllFeatures { get; set; }
    }
}