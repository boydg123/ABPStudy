using System;
using System.Globalization;

namespace Abp.Localization.Sources
{
    /// <summary>
    /// Extension methods for <see cref="ILocalizationSource"/>.
    /// <see cref="ILocalizationSource"/>的扩展方法
    /// </summary>
    public static class LocalizationSourceExtensions
    {
        /// <summary>
        /// Get a localized string by formatting string.
        /// 通过格式化字符获取本地化字符串
        /// </summary>
        /// <param name="source">Localization source / 本地化源</param>
        /// <param name="name">Key name / 键名</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Formatted and localized string / 格式化后的本地化参数</returns>
        public static string GetString(this ILocalizationSource source, string name, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return string.Format(source.GetString(name), args);
        }

        /// <summary>
        /// Get a localized string in given language by formatting string.
        /// 通过格式化字符获取给定语言的本地化字符串
        /// </summary>
        /// <param name="source">Localization source / 本地化源</param>
        /// <param name="name">Key name / 键名</param>
        /// <param name="culture">Culture / 区域区域</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Formatted and localized string / 格式化后的本地化参数</returns>
        public static string GetString(this ILocalizationSource source, string name, CultureInfo culture, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return string.Format(source.GetString(name, culture), args);
        }
    }
}