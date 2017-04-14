using System;
using System.Globalization;

namespace Abp.Localization
{
    /// <summary>
    /// Represents a string that can be localized.
    /// 表示一个在需要时能本地化的字符串
    /// </summary>
    [Serializable]
    public class LocalizableString : ILocalizableString
    {
        /// <summary>
        /// Unique name of the localization source.
        /// 本地化源名称
        /// </summary>
        public virtual string SourceName { get; private set; }

        /// <summary>
        /// Unique Name of the string to be localized.
        /// 被本地化的字符串的名称
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// Needed for serialization.
        /// 为了序列化
        /// </summary>
        private LocalizableString()
        {
            
        }

        /// <param name="name">Unique Name of the string to be localized / 本地化源名称</param>
        /// <param name="sourceName">Unique name of the localization source / 将被本地化的字符串名称</param>
        public LocalizableString(string name, string sourceName)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (sourceName == null)
            {
                throw new ArgumentNullException("sourceName");
            }

            Name = name;
            SourceName = sourceName;
        }

        /// <summary>
        /// 使用当前语言本地化字符串
        /// </summary>
        /// <returns>本地化后的字符串</returns>
        public string Localize(ILocalizationContext context)
        {
            return context.LocalizationManager.GetString(SourceName, Name);
        }

        /// <summary>
        /// 使用当前语言本地化字符串
        /// </summary>
        /// <param name="culture">区域</param>
        /// <returns>本地化后的字符串</returns>
        public string Localize(ILocalizationContext context, CultureInfo culture)
        {
            return context.LocalizationManager.GetString(SourceName, Name, culture);
        }

        public override string ToString()
        {
            return string.Format("[LocalizableString: {0}, {1}]", Name, SourceName);
        }
    }
}
