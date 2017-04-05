using System.Collections.Generic;

namespace Abp.Localization.Dictionaries
{
    /// <summary>
    /// Used to get localization dictionaries (<see cref="ILocalizationDictionary"/>) for a <see cref="IDictionaryBasedLocalizationSource"/>.
    /// 用于为<see cref="IDictionaryBasedLocalizationSource"/>.获取本地化字典(<see cref="ILocalizationDictionary"/>)
    /// </summary>
    public interface ILocalizationDictionaryProvider
    {
        /// <summary>
        /// 本地化字典
        /// </summary>
        ILocalizationDictionary DefaultDictionary { get; }

        IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName"></param>
        void Initialize(string sourceName);
        
        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="dictionary"></param>
        void Extend(ILocalizationDictionary dictionary);
    }
}