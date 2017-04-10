using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP Web模块配置实现
    /// </summary>
    public class AbpWebModuleConfiguration : IAbpWebModuleConfiguration
    {
        /// <summary>
        /// ABP Web防伪配置
        /// </summary>
        public IAbpAntiForgeryWebConfiguration AntiForgery { get; }

        /// <summary>
        /// ABP Web本地化配置
        /// </summary>
        public IAbpWebLocalizationConfiguration Localization { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="antiForgery">ABP Web防伪配置</param>
        /// <param name="localization">ABP Web本地化配置</param>
        public AbpWebModuleConfiguration(
            IAbpAntiForgeryWebConfiguration antiForgery, 
            IAbpWebLocalizationConfiguration localization)
        {
            AntiForgery = antiForgery;
            Localization = localization;
        }
    }
}