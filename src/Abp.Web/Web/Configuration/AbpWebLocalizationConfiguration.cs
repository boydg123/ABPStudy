namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP Web本地化配置
    /// </summary>
    public class AbpWebLocalizationConfiguration : IAbpWebLocalizationConfiguration
    {
        /// <summary>
        /// Default: "Abp.Localization.CultureName".
        /// CookieName.默认值："Abp.Localization.CultureName"
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpWebLocalizationConfiguration()
        {
            CookieName = "Abp.Localization.CultureName";
        }
    }
}