using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 商户列表Dto
    /// </summary>
    [AutoMapFrom(typeof (Tenant))]
    public class TenantListDto : EntityDto, IPassivable, IHasCreationTime
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 版本显示名
        /// </summary>
        public string EditionDisplayName { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}