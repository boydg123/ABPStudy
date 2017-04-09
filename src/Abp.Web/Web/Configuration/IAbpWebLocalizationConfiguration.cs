namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP Web本地化配置
    /// </summary>
    public interface IAbpWebLocalizationConfiguration
    {
        /// <summary>
        /// Cookie 名称
        /// </summary>
        string CookieName { get; set; }
    }
}