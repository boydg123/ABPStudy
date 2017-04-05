using System.ComponentModel.DataAnnotations;

namespace Derrick.Configuration.Host.Dto
{
    public class GeneralSettingsEditDto
    {
        [MaxLength(128)]
        public string WebSiteRootAddress { get; set; }

        public string Timezone { get; set; }

        /// <summary>
        /// This value is only used for comparing user's timezone to default timezone
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}