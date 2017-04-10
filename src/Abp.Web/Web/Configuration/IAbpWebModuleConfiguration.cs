using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP Web模块配置
    /// </summary>
    public interface IAbpWebModuleConfiguration
    {
        /// <summary>
        /// ABP Web防伪配置
        /// </summary>
        IAbpAntiForgeryWebConfiguration AntiForgery { get; }

        /// <summary>
        /// ABP Web本地化配置
        /// </summary>
        IAbpWebLocalizationConfiguration Localization { get; }
    }
}