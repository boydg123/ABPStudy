using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Derrick.Editions.Dto
{
    /// <summary>
    /// 版本编辑Output
    /// </summary>
    public class GetEditionForEditOutput
    {
        /// <summary>
        /// 版本编辑Dto
        /// </summary>
        public EditionEditDto Edition { get; set; }
        /// <summary>
        /// 功能值Dto
        /// </summary>
        public List<NameValueDto> FeatureValues { get; set; }
        /// <summary>
        /// 平级功能列表
        /// </summary>
        public List<FlatFeatureDto> Features { get; set; }
    }
}