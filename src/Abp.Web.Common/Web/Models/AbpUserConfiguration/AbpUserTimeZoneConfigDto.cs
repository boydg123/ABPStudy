namespace Abp.Web.Models.AbpUserConfiguration
{
    /// <summary>
    /// ABP用户时区配置Dto
    /// </summary>
    public class AbpUserTimeZoneConfigDto
    {
        public AbpUserWindowsTimeZoneConfigDto Windows { get; set; }

        public AbpUserIanaTimeZoneConfigDto Iana { get; set; }
    }
}