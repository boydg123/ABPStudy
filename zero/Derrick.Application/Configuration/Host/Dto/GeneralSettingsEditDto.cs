using System.ComponentModel.DataAnnotations;

namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 常规设置编辑Dto
    /// </summary>
    public class GeneralSettingsEditDto
    {
        /// <summary>
        /// 网站根地址
        /// </summary>
        [MaxLength(128)]
        public string WebSiteRootAddress { get; set; }
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// This value is only used for comparing user's timezone to default timezone
        /// 这个值仅用于比较用户的时区默认时区
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}