using System.Globalization;

namespace Abp.Localization
{
    /// <summary>
    /// Represents a localized string.
    /// 表示一个本地化字符串
    /// </summary>
    public class LocalizedString
    {
        /// <summary>
        /// Culture info for this string.
        /// 该字符串的区域
        /// </summary>
        public CultureInfo CultureInfo { get; internal set; }

        /// <summary>
        /// Unique Name of the string.
        /// 字符串的唯一名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Value for the <see cref="Name"/>.
        /// 名称 <see cref="Name"/>对应的本地化字符串值.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Creates a localized string instance.
        /// 创建一个本地化字符串实例
        /// </summary>
        /// <param name="cultureInfo">Culture info for this string / 该字符串的区域</param>
        /// <param name="name">Unique Name of the string / 字符串的唯一名称</param>
        /// <param name="value">Value for the <paramref name="name"/> / 名称 <see cref="Name"/>对应的本地化字符串值.</param>
        public LocalizedString(string name, string value, CultureInfo cultureInfo)
        {
            Name = name;
            Value = value;
            CultureInfo = cultureInfo;
        }
    }
}