using System.Globalization;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    /// <summary>
    /// Extends <see cref="ILocalizationSource"/> to add tenant and database based localization.
    /// <see cref="ILocalizationSource"/>的扩展，添加了商户和基于数据库的本地化
    /// </summary>
    public interface IMultiTenantLocalizationSource : ILocalizationSource
    {
        /// <summary>
        /// Gets a <see cref="LocalizedString"/>.
        /// 获取<see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host. / 商户ID或Null(宿主商户)</param>
        /// <param name="name">Localization key name. / 本地化Key名称</param>
        /// <param name="culture">Culture / 区域信息</param>
        string GetString(int? tenantId, string name, CultureInfo culture);

        /// <summary>
        /// Gets a <see cref="LocalizedString"/>.
        /// 获取<see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host. / 商户ID或Null(宿主商户)</param>
        /// <param name="name">Localization key name. / 本地化Key名称</param>
        /// <param name="culture">Culture / 区域信息</param>
        /// <param name="tryDefaults">True: fallbacks to default languages if can not find in given culture / True：如果根据指定的区域信息不能找到则返回默认语言</param>
        string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true);
    }
}