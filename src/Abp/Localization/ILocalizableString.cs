using System.Globalization;

namespace Abp.Localization
{
    /// <summary>
    /// Represents a string that can be localized when needed.
    /// 表示一个在需要时能本地化的字符串
    /// </summary>
    public interface ILocalizableString
    {
        /// <summary>
        /// Localizes the string in current culture.
        /// 在当前文化的字符串
        /// </summary>
        /// <param name="context">Localization context / 本地化上下文</param>
        /// <returns>Localized string / 本地化字符串</returns>
        string Localize(ILocalizationContext context);

        /// <summary>
        /// Localizes the string in given culture.
        /// 给定文化的本地化字符串
        /// </summary>
        /// <param name="context">Localization context / 本地化上下文</param>
        /// <param name="culture">culture / 文化信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        string Localize(ILocalizationContext context, CultureInfo culture);
    }
}