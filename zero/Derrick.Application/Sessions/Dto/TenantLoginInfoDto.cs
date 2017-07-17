using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Derrick.MultiTenancy;

namespace Derrick.Sessions.Dto
{
    /// <summary>
    /// 商户登录信息Dto
    /// </summary>
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        /// <summary>
        /// 商户名
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
    }
}