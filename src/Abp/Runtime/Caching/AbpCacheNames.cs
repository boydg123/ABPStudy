namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Names of standard caches used in ABP.
    /// ABP中使用的标准缓存名称
    /// </summary>
    public static class AbpCacheNames
    {
        /// <summary>
        /// Application settings cache: AbpApplicationSettingsCache.
        /// 应用程序设置缓存：AbpApplicationSettingsCache
        /// </summary>
        public const string ApplicationSettings = "AbpApplicationSettingsCache";

        /// <summary>
        /// Tenant settings cache: AbpTenantSettingsCache.
        /// 租户设置缓存：AbpTenantSettingsCache
        /// </summary>
        public const string TenantSettings = "AbpTenantSettingsCache";

        /// <summary>
        /// User settings cache: AbpUserSettingsCache.
        /// 用户设置缓存：AbpUserSettingsCache
        /// </summary>
        public const string UserSettings = "AbpUserSettingsCache";

        /// <summary>
        /// Localization scripts cache: AbpLocalizationScripts.
        /// 本地化脚本缓存：AbpLocalizationScripts
        /// </summary>
        public const string LocalizationScripts = "AbpLocalizationScripts";
    }
}