using System.Collections.Generic;
using Abp.Localization.Dictionaries;

namespace Abp.Localization
{
    /// <summary>
    /// Extends <see cref="ILocalizationDictionary"/> to add tenant and database based localization.
    /// <see cref="ILocalizationDictionary"/>的扩展，添加了商户和基于数据库的本地化
    /// </summary>
    public interface IMultiTenantLocalizationDictionary : ILocalizationDictionary
    {
        /// <summary>
        /// 获取一个<see cref="LocalizedString"/>
        /// </summary>
        /// <param name="tenantId">TenantId or null for host. / 商户ID或Null(宿主商户)</param>
        /// <param name="name">Localization key name. / 本地化Key名称</param>
        LocalizedString GetOrNull(int? tenantId, string name);

        /// <summary>
        /// Gets all <see cref="LocalizedString"/>s.
        /// 获取所有的<see cref="LocalizedString"/>
        /// </summary>
        /// <param name="tenantId">TenantId or null for host. / 商户ID或Null(宿主商户)</param>
        IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId);
    }
}