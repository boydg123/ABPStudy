using System.Collections.Generic;

namespace Abp.Localization.Dictionaries.Xml
{
    /// <summary>
    /// 本地字典提供者基类
    /// </summary>
    public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
    {
        /// <summary>
        /// 源名称
        /// </summary>
        public string SourceName { get; private set; }

        /// <summary>
        /// 本地化字典
        /// </summary>
        public ILocalizationDictionary DefaultDictionary { get; protected set; }

        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected LocalizationDictionaryProviderBase()
        {
            Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName"></param>
        public virtual void Initialize(string sourceName)
        {
            SourceName = sourceName;
        }

        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="dictionary">本地字典</param>
        public void Extend(ILocalizationDictionary dictionary)
        {
            //Add
            ILocalizationDictionary existingDictionary;
            if (!Dictionaries.TryGetValue(dictionary.CultureInfo.Name, out existingDictionary))
            {
                Dictionaries[dictionary.CultureInfo.Name] = dictionary;
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