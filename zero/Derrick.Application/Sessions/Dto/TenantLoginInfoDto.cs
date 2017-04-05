using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Derrick.MultiTenancy;

namespace Derrick.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string EditionDisplayName { get; set; }
    }
}