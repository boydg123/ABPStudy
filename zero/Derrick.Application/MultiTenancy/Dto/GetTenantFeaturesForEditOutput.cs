using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Derrick.Editions.Dto;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 商户功能编辑Output
    /// </summary>
    public class GetTenantFeaturesForEditOutput
    {
        /// <summary>
        /// 功能值列表
        /// </summary>
        public List<NameValueDto> FeatureValues { get; set; }
        /// <summary>
        /// 功能列表
        /// </summary>
        public List<FlatFeatureDto> Features { get; set; }
    }
}