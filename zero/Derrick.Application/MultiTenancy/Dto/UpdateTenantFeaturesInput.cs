using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 更新商户功能Input
    /// </summary>
    public class UpdateTenantFeaturesInput
    {
        /// <summary>
        /// ID
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        /// <summary>
        /// 功能值列表
        /// </summary>
        [Required]
        public List<NameValueDto> FeatureValues { get; set; }
    }
}