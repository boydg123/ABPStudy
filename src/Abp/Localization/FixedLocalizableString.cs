using System;
using System.Globalization;

namespace Abp.Localization
{
    /// <summary>
    /// A class that gets the same string on every localization.
    /// 在每个本地化都能获取相同的字符串
    /// </summary>
    [Serializable]
    public class FixedLocalizableString : ILocalizableString
    {
        /// <summary>
        /// The fixed string.Whenever Localize methods called, this string is returned.
        /// 固定的字符串，不管本地化方法是否调用，都返回此字符串
        /// </summary>
        public virtual string FixedString { get; private set; }

        /// <summary>
        /// Needed for serialization.
        /// 为了序列化
        /// </summary>
        private FixedLocalizableString()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="FixedLocalizableString"/>.
        /// 构造函数
        /// </summary>
        /// <param name="fixedString">
        /// The fixed string.Whenever Localize methods called, this string is returned.
        /// 固定的字符串，不管本地化方法是否调用，都返回此字符串
        /// </param>
        public FixedLocalizableString(string fixedString)
        {
            FixedString = fixedString;
        }

        /// <summary>
        /// 总是获取 <see cref="FixedString"/> 字符串.
        /// </summary>
        /// <param name="context">本地化上下文</param>
        /// <returns></returns>
        public string Localize(ILocalizationContext context)
        {
            return FixedString;
        }

        /// <summary>
        /// 总是获取 <see cref="FixedString"/> 字符串.
        /// </summary>
        public string Localize(ILocalizationContext context, CultureInfo culture)
        {
            return FixedString;
        }

        public override string ToString()
        {
            return FixedString;
        }
    }
}