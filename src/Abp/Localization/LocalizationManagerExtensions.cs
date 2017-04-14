using System.Globalization;

namespace Abp.Localization
{
    /// <summary>
    /// <see cref="LocalizationManager"/>的扩展
    /// </summary>
    public static class LocalizationManagerExtensions
    {
        /// <summary>
        /// Gets a localized string in current language.
        /// 获取当前语言中的本地化字符串
        /// </summary>
        /// <returns>Localized string / 本地化字符串</returns>
        public static string GetString(this ILocalizationManager localizationManager, LocalizableString localizableString)
        {
            return localizationManager.GetString(localizableString.SourceName, localizableString.Name);
        }

        /// <summary>
        /// Gets a localized string in specified language.
        /// 在特定语言中获取本地化字符串
        /// </summary>
        /// <returns>Localized string / 本地化字符串</returns>
        public static string GetString(this ILocalizationManager localizationManager, LocalizableString localizableString, CultureInfo culture)
        {
            return localizationManager.GetString(localizableString.SourceName, localizableString.Name, culture);
        }

        /// <summary>
        /// Gets a localized string in current language.
        /// 获取当前语言中的本地化字符串
        /// </summary>
        /// <param name="localizationManager">Localization manager instance / 本地化管理器的实例</param>
        /// <param name="sourceName">Name of the localization source / 本地化源的名称</param>
        /// <param name="name">Key name to get localized string / 获取本地化字符串的key名称</param>
        /// <returns>Localized string / 本地化字符串</returns>
        public static string GetString(this ILocalizationManager localizationManager, string sourceName, string name)
        {
            return localizationManager.GetSource(sourceName).GetString(name);
        }

        /// <summary>
        /// Gets a localized string in specified language.
        /// 在特定语言中获取本地化字符串
        /// </summary>
        /// <param name="localizationManager">Localization manager instance / 本地化管理器的实例</param>
        /// <param name="sourceName">Name of the localization source / 本地化源的名称</param>
        /// <param name="name">Key name to get localized string / 获取本地化字符串的key名称</param>
        /// <param name="culture">culture / 区域信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        public static string GetString(this ILocalizationManager localizationManager, string sourceName, string name, CultureInfo culture)
        {
            return localizationManager.GetSource(sourceName).GetString(name, culture);
        }
    }
}