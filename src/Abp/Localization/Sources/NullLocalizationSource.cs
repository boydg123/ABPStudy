using System.Collections.Generic;
using System.Globalization;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Localization.Sources
{
    /// <summary>
    /// Null object pattern for <see cref="ILocalizationSource"/>.
    /// <see cref="ILocalizationSource"/>的NULl对象模式实现
    /// </summary>
    public class NullLocalizationSource : ILocalizationSource
    {
        /// <summary>
        /// Singleton instance.
        /// 单例
        /// </summary>
        public static NullLocalizationSource Instance { get { return SingletonInstance; } }
        private static readonly NullLocalizationSource SingletonInstance = new NullLocalizationSource();

        /// <summary>
        /// 唯一的名称
        /// </summary>
        public string Name { get { return null; } }

        /// <summary>
        /// 本地化字符串数组
        /// </summary>
        private readonly IReadOnlyList<LocalizedString> _emptyStringArray = new LocalizedString[0];

        /// <summary>
        /// 构造函数
        /// </summary>
        private NullLocalizationSource()
        {
            
        }

        /// <summary>
        /// 该方法在abp第一次使用前调用
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="iocResolver"></param>
        public void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            
        }

        /// <summary>
        ///获取给定名称的当前语言表示的字符串
        /// </summary>
        /// <param name="name">键名称</param>
        /// <returns>本地化字符串</returns>
        public string GetString(string name)
        {
            return name;
        }

        /// <summary>
        //获取给定名称和区域的本地化字符串
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="culture">区域信息</param>
        /// <returns>本地化字符串</returns>
        public string GetString(string name, CultureInfo culture)
        {
            return name;
        }

        /// <summary>
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="tryDefaults">true:回退到默认语言如果在当前区域没有发现</param>
        /// <returns>本地化字符串</returns>
        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            return null;
        }

        /// <summary>
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="culture">区域信息</param>
        /// <param name="tryDefaults">true:回退到默认语言如果在当前区域没有发现</param>
        /// <returns>本地化字符串</returns>
        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            return null;
        }

        /// <summary>
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="includeDefaults">true:回退到默认语言文本如果当前区域没有发现。</param>
        /// <returns></returns>
        public IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return _emptyStringArray;
        }

        /// <summary>
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="culture">区域信息</param>
        /// <param name="includeDefaults">true:回退到默认语言文本如果当前区域没有发现。</param>
        /// <returns></returns>
        public IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            return _emptyStringArray;
        }
    }
}