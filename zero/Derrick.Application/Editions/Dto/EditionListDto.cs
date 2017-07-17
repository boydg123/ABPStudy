using System;
using Abp.Application.Editions;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace Derrick.Editions.Dto
{
    /// <summary>
    /// 版本列表Dto
    /// </summary>
    [AutoMapFrom(typeof(Edition))]
    public class EditionListDto : EntityDto, IHasCreationTime
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}