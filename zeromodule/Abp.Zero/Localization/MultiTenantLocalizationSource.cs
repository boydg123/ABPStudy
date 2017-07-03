using System;
using System.Globalization;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization.Dictionaries;
using Castle.Core.Logging;

namespace Abp.Localization
{
    /// <summary>
    /// 多商户本地化源
    /// </summary>
    public class MultiTenantLocalizationSource : DictionaryBasedLocalizationSource, IMultiTenantLocalizationSource
    {
        /// <summary>
        /// 多商户本地化字典提供者
        /// </summary>
        public new MultiTenantLocalizationDictionaryProvider DictionaryProvider
        {
            get { return base.DictionaryProvider.As<MultiTenantLocalizationDictionaryProvider>(); }
        }
        /// <summary>
        /// 日志记录器引用
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="dictionaryProvider">多商户本地化字典提供者</param>
        public MultiTenantLocalizationSource(string name, MultiTenantLocalizationDictionaryProvider dictionaryProvider) 
            : base(name, dictionaryProvider)
        {
            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration">本地化配置对象</param>
        /// <param name="iocResolver">IOC解析器</param>
        public override void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            base.Initialize(configuration, iocResolver);

            if (Logger is NullLogger && iocResolver.IsRegistered(typeof(ILoggerFactory)))
            {
                Logger = iocResolver.Resolve<ILoggerFactory>().Create(typeof (MultiTenantLocalizationSource));
            }
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="name">名称</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public string GetString(int? tenantId, string name, CultureInfo culture)
        {
            var value = GetStringOrNull(tenantId, name, culture);

            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, culture);
            }

            return value;
        }
        /// <summary>
        /// 获取字符串或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="name">名称</param>
        /// <param name="culture">区域信息</param>
        /// <param name="tryDefaults">尝试默认</param>
        /// <returns></returns>
        public string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureName = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            ILocalizationDictionary originalDictionary;
            if (dictionaries.TryGetValue(cultureName, out originalDictionary))
            {
                var strOriginal = originalDictionary
                    .As<IMultiTenantLocalizationDictionary>()
                    .GetOrNull(tenantId, name);

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
                    var strLang = langDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
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

            var strDefault = defaultDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
            if (strDefault == null)
            {
                return null;
            }

            return strDefault.Value;
        }
        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="dictionary">本地化字典</param>
        public override void Extend(ILocalizationDictionary dictionary)
        {
            DictionaryProvider.Extend(dictionary);
        }
        /// <summary>
        /// 获取基本区域名称
        /// </summary>
        /// <param name="cultureName">区域名称</param>
        /// <returns></returns>
        private static string GetBaseCultureName(string cultureName)
        {
            return cultureName.Contains("-")
                ? cultureName.Left(cultureName.IndexOf("-", StringComparison.InvariantCulture))
                : cultureName;
        }
    }
}