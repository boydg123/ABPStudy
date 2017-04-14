using System.Collections.Generic;
using System.Globalization;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Localization.Sources
{
    /// <summary>
    /// A Localization Source is used to obtain localized strings.
    /// 一个Localization Source，用于获取本地化字符串
    /// </summary>
    public interface ILocalizationSource
    {
        /// <summary>
        /// Unique Name of the source.
        /// 唯一的名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This method is called by ABP before first usage.
        /// 该方法在abp第一次使用前调用
        /// </summary>
        void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver);

        /// <summary>
        /// Gets localized string for given name in current language.Fallbacks to default language if not found in current culture.
        /// 获取给定名称的当前语言表示的字符串。如果没有当前的区域信息则返回默认语言
        /// </summary>
        /// <param name="name">Key name / 键名称</param>
        /// <returns>Localized string / 本地化字符串</returns>
        string GetString(string name);

        /// <summary>
        /// Gets localized string for given name and specified culture.Fallbacks to default language if not found in given culture.
        /// 获取给定名称和区域的本地化字符串。如果没有当前的区域信息则返回默认语言
        /// </summary>
        /// <param name="name">Key name / 键名称</param>
        /// <param name="culture">culture information / 区域信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        string GetString(string name, CultureInfo culture);

        /// <summary>
        /// Gets localized string for given name in current language.Returns null if not found.
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">Key name / 键名称</param>
        /// <param name="tryDefaults">
        /// True: Fallbacks to default language if not found in current culture.
        /// true:回退到默认语言如果在当前区域没有发现
        /// </param>
        /// <returns>Localized string / 本地化字符串</returns>
        string GetStringOrNull(string name, bool tryDefaults = true);

        /// <summary>
        /// Gets localized string for given name and specified culture.Returns null if not found.
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">Key name / 键名称</param>
        /// <param name="culture">culture information / 区域信息</param>
        /// <param name="tryDefaults">
        /// True: Fallbacks to default language if not found in current culture.
        /// true:回退到默认语言如果在当前区域没有发现
        /// </param>
        /// <returns>Localized string / 本地化字符串</returns>
        string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true);

        /// <summary>
        /// Gets all strings in current language.
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="includeDefaults">
        /// True: Fallbacks to default language texts if not found in current culture.
        /// 回退到默认语言文本如果当前区域没有发现。
        /// </param>
        IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true);

        /// <summary>
        /// Gets all strings in specified culture.
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="culture">culture information / 区域信息</param>
        /// <param name="includeDefaults">
        /// True: Fallbacks to default language texts if not found in current culture.
        /// 回退到默认语言文本如果当前区域没有发现
        /// </param>
        IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true);
    }
}