using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP Web模块配置
    /// </summary>
    public interface IAbpWebModuleConfiguration
    {
        IAbpAntiForgeryWebConfiguration AntiForgery { get; }

        IAbpWebLocalizationConfiguration Localization { get; }
    }
}