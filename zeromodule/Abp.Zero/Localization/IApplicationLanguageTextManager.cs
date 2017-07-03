using System.Globalization;
using System.Threading.Tasks;

namespace Abp.Localization
{
    /// <summary>
    /// Manages localization texts for host and tenants.
    /// 管理宿主和租户的本地化文本
    /// </summary>
    public interface IApplicationLanguageTextManager
    {
        /// <summary>
        /// Gets a localized string value.
        /// 获取一个本地化字符串值
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(宿主商户)</param>
        /// <param name="sourceName">Source name / 源名称</param>
        /// <param name="culture">Culture / 区域文化信息</param>
        /// <param name="key">Localization key / 本地化字符串Key</param>
        /// <param name="tryDefaults">True: fallbacks to default languages if can not find in given culture / True：如果在指定的区域中没有找到则返回默认语言</param>
        string GetStringOrNull(int? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true);

        /// <summary>
        /// Updates a localized string value.
        /// 更新一个本地化字符串值
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(宿主商户)</param>
        /// <param name="sourceName">Source name / 源名称</param>
        /// <param name="culture">Culture / 区域文化信息</param>
        /// <param name="key">Localization key / 本地化字符串Key</param>
        /// <param name="value">New localized value. / 新本地化值</param>
        Task UpdateStringAsync(int? tenantId, string sourceName, CultureInfo culture, string key, string value);
    }
}