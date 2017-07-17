using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Editions.Dto
{
    /// <summary>
    /// 创建或更新版本Dto
    /// </summary>
    public class CreateOrUpdateEditionDto
    {
        /// <summary>
        /// 版本编辑Dto
        /// </summary>
        [Required]
        public EditionEditDto Edition { get; set; }

        /// <summary>
        /// 功能值列表
        /// </summary>
        [Required]
        public List<NameValueDto> FeatureValues { get; set; }
    }
}