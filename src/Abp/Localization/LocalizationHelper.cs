using System;
using System.Globalization;
using Abp.Dependency;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    /// <summary>
    /// This static class is used to simplify getting localized strings.
    /// 这个静态类用于简化获取本地化字符串
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Gets a reference to the localization manager.Inject and use <see cref="ILocalizationManager"/> wherever it's possible, instead of this shortcut.
        /// 本地化管理器的引用，使用<see cref="ILocalizationManager"/>注入
        /// </summary>
        public static ILocalizationManager Manager { get { return LocalizationManager.Value; } }

        private static readonly Lazy<ILocalizationManager> LocalizationManager;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LocalizationHelper()
        {
            LocalizationManager = new Lazy<ILocalizationManager>(
                () => IocManager.Instance.IsRegistered<ILocalizationManager>()
                    ? IocManager.Instance.Resolve<ILocalizationManager>()
                    : NullLocalizationManager.Instance
                );
        }

        /// <summary>
        /// Gets a pre-registered localization source.
        /// 获取一个预先注册的localization source.
        /// </summary>
        public static ILocalizationSource GetSource(string name)
        {
            return LocalizationManager.Value.GetSource(name);
        }

        /// <summary>
        /// Gets a localized string in current language.
        /// 在当前语言中获取一个本地化的字符串
        /// </summary>
        /// <param name="sourceName">Name of the localization source / localization source的名称</param>
        /// <param name="name">Key name to get localized string / 获取本地化字符的key名称</param>
        /// <returns>Localized strin / 本地化字符串</returns>
        public static string GetString(string sourceName, string name)
        {
            return LocalizationManager.Value.GetString(sourceName, name);
        }

        /// <summary>
        /// Gets a localized string in specified language.
        /// 在特定的语言中下获取本地化字符串
        /// </summary>
        /// <param name="sourceName">Name of the localization source / localization source的名称</param>
        /// <param name="name">Key name to get localized string / 获取本地化字符的key名称</param>
        /// <param name="culture">culture / 区域</param>
        /// <returns>Localized string / 本地化字符串</returns>
        public static string GetString(string sourceName, string name, CultureInfo culture)
        {
            return LocalizationManager.Value.GetString(sourceName, name, culture);
        }
    }
}