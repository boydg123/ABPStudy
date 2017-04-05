using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Localization.Dictionaries
{
    /// <summary>
    /// This class is used to build a localization source which works on memory based dictionaries to find strings.
    /// 这个类是用来建立一个本地的资源，它在基于内存的字典上查找字符串
    /// </summary>
    public class DictionaryBasedLocalizationSource : IDictionaryBasedLocalizationSource
    {
        /// <summary>
        /// Unique Name of the source.
        /// 资源的唯一名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 本地化字典提供者
        /// </summary>
        public ILocalizationDictionaryProvider DictionaryProvider { get { return _dictionaryProvider; } }

        /// <summary>
        /// 本地化配置
        /// </summary>
        protected ILocalizationConfiguration LocalizationConfiguration { get; private set; }

        /// <summary>
        /// 本地化字典提供者
        /// </summary>
        private readonly ILocalizationDictionaryProvider _dictionaryProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="dictionaryProvider">本地化字典提供者</param>
        public DictionaryBasedLocalizationSource(string name, ILocalizationDictionaryProvider dictionaryProvider)
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentNullException("name");
            }

            Name = name;

            if (dictionaryProvider == null)
            {
                throw new ArgumentNullException("dictionaryProvider");
            }

            _dictionaryProvider = dictionaryProvider;
        }

        /// <summary>
        /// 初始化(这个方法在第一次使用之前由ABP调用)
        /// </summary>
        /// <param name="configuration">背地花配置</param>
        /// <param name="iocResolver">IOC解析器</param>
        public virtual void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            LocalizationConfiguration = configuration;
            DictionaryProvider.Initialize(Name);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetString(string name)
        {
            return GetString(name, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="culture">文化信息</param>
        /// <returns></returns>
        public string GetString(string name, CultureInfo culture)
        {
            var value = GetStringOrNull(name, culture);

            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, culture);
            }

            return value;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="tryDefaults">是否默认</param>
        /// <returns>如果没有找到则返回null</returns>
        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            return GetStringOrNull(name, Thread.CurrentThread.CurrentUICulture, tryDefaults);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="culture">文化信息</param>
        /// <param name="tryDefaults">是否默认</param>
        /// <returns>如果没有找到则返回null</returns>
        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureName = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            ILocalizationDictionary originalDictionary;
            if (dictionaries.TryGetValue(cultureName, out originalDictionary))
            {
                var strOriginal = originalDictionary.GetOrNull(name);
                if (strOriginal != null)
                {
                    return strOriginal.Value;
                }
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                ILocalizationDictionary langDictionary;
                if (dictionaries.TryGetValue(GetBaseCultureName(cultureName), out langDictionary))
                {
                    var strLang = langDictionary.GetOrNull(name);
                    if (strLang != null)
                    {
                        return strLang.Value;
                    }
                }
            }

            //Try to get from default language
            var defaultDictionary = DictionaryProvider.DefaultDictionary;
            if (defaultDictionary == null)
            {
                return null;
            }

            var strDefault = defaultDictionary.GetOrNull(name);
            if (strDefault == null)
            {
                return null;
            }

            return strDefault.Value;
        }

        /// <summary>
        /// 获取当前语言中的所有字符串
        /// </summary>
        /// <param name="includeDefaults">回退到默认语言文本如果当前文化没有发现。</param>
        /// <returns></returns>
        public IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return GetAllStrings(Thread.CurrentThread.CurrentUICulture, includeDefaults);
        }

        /// <summary>
        /// 获取指定文化中的所有字符串
        /// </summary>
        /// <param name="culture">文化信息</param>
        /// <param name="includeDefaults">回退到默认语言文本如果当前文化没有发现。</param>
        /// <returns></returns>
        public IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            //TODO: Can be optimized (example: if it's already default dictionary, skip overriding)

            var dictionaries = DictionaryProvider.Dictionaries;

            //Create a temp dictionary to build
            var allStrings = new Dictionary<string, LocalizedString>();

            if (includeDefaults)
            {
                //Fill all strings from default dictionary
                var defaultDictionary = DictionaryProvider.DefaultDictionary;
                if (defaultDictionary != null)
                {
                    foreach (var defaultDictString in defaultDictionary.GetAllStrings())
                    {
                        allStrings[defaultDictString.Name] = defaultDictString;
                    }
                }

                //Overwrite all strings from the language based on country culture
                if (culture.Name.Contains("-"))
                {
                    ILocalizationDictionary langDictionary;
                    if (dictionaries.TryGetValue(GetBaseCultureName(culture.Name), out langDictionary))
                    {
                        foreach (var langString in langDictionary.GetAllStrings())
                        {
                            allStrings[langString.Name] = langString;
                        }
                    }
                }
            }

            //Overwrite all strings from the original dictionary
            ILocalizationDictionary originalDictionary;
            if (dictionaries.TryGetValue(culture.Name, out originalDictionary))
            {
                foreach (var originalLangString in originalDictionary.GetAllStrings())
                {
                    allStrings[originalLangString.Name] = originalLangString;
                }
            }

            return allStrings.Values.ToImmutableList();
        }

        /// <summary>
        /// Extends the source with given dictionary.
        /// 为给定的字典扩展源
        /// </summary>
        /// <param name="dictionary">Dictionary to extend the source / 扩展源的字典</param>
        public virtual void Extend(ILocalizationDictionary dictionary)
        {
            DictionaryProvider.Extend(dictionary);
        }

        /// <summary>
        /// 返回给定的名称或抛出异常
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="culture">文化信息</param>
        /// <returns></returns>
        protected virtual string ReturnGivenNameOrThrowException(string name, CultureInfo culture)
        {
            return LocalizationSourceHelper.ReturnGivenNameOrThrowException(LocalizationConfiguration, Name, name, culture);
        }

        /// <summary>
        /// 获取基础文化名称
        /// </summary>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        private static string GetBaseCultureName(string cultureName)
        {
            return cultureName.Contains("-")
                ? cultureName.Left(cultureName.IndexOf("-", StringComparison.InvariantCulture))
                : cultureName;
        }
    }
}
