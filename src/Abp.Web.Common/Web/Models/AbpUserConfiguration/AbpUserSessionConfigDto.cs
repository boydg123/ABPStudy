using Abp.MultiTenancy;

namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP”√ªßSession≈‰÷√Dto
    /// </summary>
    public class AbpUserSessionConfigDto
    {
        public long? UserId { get; set; }

        public int? TenantId { get; set; }

        public long? ImpersonatorUserId { get; set; }

        public int? ImpersonatorTenantId { get; set; }

        public MultiTenancySides MultiTenancySide { get; set; }
    }
}