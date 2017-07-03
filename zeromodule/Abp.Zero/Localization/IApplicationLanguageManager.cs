using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Localization
{
    /// <summary>
    /// Manages host and tenant languages.
    /// 管理宿主和商户的语言
    /// </summary>
    public interface IApplicationLanguageManager
    {
        /// <summary>
        /// Gets list of all languages available to given tenant (or null for host)
        /// 获取给定商户可用的所有语言列表。(host的为null)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(商户是宿主商户)</param>
        Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId);

        /// <summary>
        /// Adds a new language.
        /// 添加一个新语言
        /// </summary>
        /// <param name="language">应用程序语言对象.</param>
        Task AddAsync(ApplicationLanguage language);

        /// <summary>
        /// Deletes a language.
        /// 删除一个语言
        /// </summary>
        /// <param name="tenantId">商户ID或Null(商户是宿主商户).</param>
        /// <param name="languageName">语言的名称.</param>
        Task RemoveAsync(int? tenantId, string languageName);

        /// <summary>
        /// Updates a language.
        /// 更新一个语言
        /// </summary>
        /// <param name="tenantId">商户ID或Null(商户是宿主商户).</param>
        /// <param name="language">The language to be updated / 将要被更新的语言对象</param>
        Task UpdateAsync(int? tenantId, ApplicationLanguage language);

        /// <summary>
        /// Gets the default language or null for a tenant or the host.
        /// 获取商户的默认语言。如果商户是宿主商户则为Null
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host / 商户ID或Null(商户是宿主商户)</param>
        Task<ApplicationLanguage> GetDefaultLanguageOrNullAsync(int? tenantId);

        /// <summary>
        /// Sets the default language for a tenant or the host.
        /// 为商户(宿主商户)设置默认语言。
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host / 商户ID或Null(商户是宿主商户)</param>
        /// <param name="languageName">Name of the language. / 语言的名称</param>
        Task SetDefaultLanguageAsync(int? tenantId, string languageName);
    }
}