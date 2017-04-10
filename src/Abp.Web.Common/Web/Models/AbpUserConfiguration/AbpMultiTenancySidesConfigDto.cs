using Abp.MultiTenancy;

namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP∂‡◊‚ªßSides≈‰÷√Dto
    /// </summary>
    public class AbpMultiTenancySidesConfigDto
    {
        public MultiTenancySides Host { get; private set; }

        public MultiTenancySides Tenant { get; private set; }

        public AbpMultiTenancySidesConfigDto()
        {
            Host = MultiTenancySides.Host;
            Tenant = MultiTenancySides.Tenant;
        }
    }
}