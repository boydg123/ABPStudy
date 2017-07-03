using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Collections.Extensions;

namespace Abp.Localization
{
    /// <summary>
    /// Extends <see cref="ILocalizationDictionaryProvider"/> to add tenant and database based localization.
    /// <see cref="ILocalizationDictionaryProvider"/>的扩展，添加了商户和基于数据库的本地化
    /// </summary>
    public class MultiTenantLocalizationDictionaryProvider : ILocalizationDictionaryProvider
    {
        /// <summary>
        /// 本地化字符串的默认字典
        /// </summary>
        public ILocalizationDictionary DefaultDictionary
        {
            get { return GetDefaultDictionary(); }
        }
        /// <summary>
        /// 本地化字典集合
        /// </summary>
        public IDictionary<string, ILocalizationDictionary> Dictionaries
        {
            get { return GetDictionaries(); }
        }
        /// <summary>
        /// 当前本地化字典集合
        /// </summary>
        private readonly ConcurrentDictionary<string, ILocalizationDictionary> _dictionaries;
        /// <summary>
        /// 源名称
        /// </summary>
        private string _sourceName;
        /// <summary>
        /// 本地化字典内部提供者
        /// </summary>
        private readonly ILocalizationDictionaryProvider _internalProvider;
        /// <summary>
        /// IOC管理引用
        /// </summary>
        private readonly IIocManager _iocManager;
        /// <summary>
        /// 语言管理引用
        /// </summary>
        private ILanguageManager _languageManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiTenantLocalizationDictionaryProvider(ILocalizationDictionaryProvider internalProvider, IIocManager iocManager)
        {
            _internalProvider = internalProvider;
            _iocManager = iocManager;
            _dictionaries = new ConcurrentDictionary<string, ILocalizationDictionary>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName">源名称</param>
        public void Initialize(string sourceName)
        {
            _sourceName = sourceName;
            _languageManager = _iocManager.Resolve<ILanguageManager>();
            _internalProvider.Initialize(_sourceName);
        }
        /// <summary>
        /// 获取字典集合
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, ILocalizationDictionary> GetDictionaries()
        {
            var languages = _languageManager.GetLanguages();

            foreach (var language in languages)
            {
                _dictionaries.GetOrAdd(language.Name, s => CreateLocalizationDictionary(language));
            }

            return _dictionaries;
        }
        /// <summary>
        /// 获取默认字典
        /// </summary>
        /// <returns></returns>
        protected virtual ILocalizationDictionary GetDefaultDictionary()
        {
            var languages = _languageManager.GetLanguages();
            if (!languages.Any())
            {
                throw new ApplicationException("No language defined!");
            }

            var defaultLanguage = languages.FirstOrDefault(l => l.IsDefault);
            if (defaultLanguage == null)
            {
                throw new ApplicationException("Default language is not defined!");
            }

            return _dictionaries.GetOrAdd(defaultLanguage.Name, s => CreateLocalizationDictionary(defaultLanguage));
        }
        /// <summary>
        /// 创建本地化字典
        /// </summary>
        /// <param name="language">语言信息</param>
        /// <returns></returns>
        protected virtual IMultiTenantLocalizationDictionary CreateLocalizationDictionary(LanguageInfo language)
        {
            var internalDictionary =
                _internalProvider.Dictionaries.GetOrDefault(language.Name) ??
                new EmptyDictionary(CultureInfo.GetCultureInfo(language.Name));

            var dictionary =  _iocManager.Resolve<IMultiTenantLocalizationDictionary>(new
            {
                sourceName = _sourceName,
                internalDictionary = internalDictionary
            });

            return dictionary;
        }
        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="dictionary">本地化字典对象</param>
        public virtual void Extend(ILocalizationDictionary dictionary)
        {
            //Add
            ILocalizationDictionary existingDictionary;
            if (!_internalProvider.Dictionaries.TryGetValue(dictionary.CultureInfo.Name, out existingDictionary))
            {
                _internalProvider.Dictionaries[dictionary.CultureInfo.Name] = dictionary;
                return;
            }

            //Override
            var localizedStrings = dictionary.GetAllStrings();
            foreach (var localizedString in localizedStrings)
            {
                existingDictionary[localizedString.Name] = localizedString.Value;
            }
        }
    }
}