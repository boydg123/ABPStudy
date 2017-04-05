using System.IO;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    ///     Provides localization dictionaries from json files in a directory.
    ///     从目录中的JSON文件提取本地化字典
    /// </summary>
    public class JsonFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
    {
        /// <summary>
        /// 目录路径
        /// </summary>
        private readonly string _directoryPath;

        /// <summary>
        ///     Creates a new <see cref="JsonFileLocalizationDictionaryProvider" />.
        ///     构造函数
        /// </summary>
        /// <param name="directoryPath">Path of the dictionary that contains all related XML files / 包含所有相关Json文件的字典路径</param>
        public JsonFileLocalizationDictionaryProvider(string directoryPath)
        {
            _directoryPath = directoryPath;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName"></param>
        public override void Initialize(string sourceName)
        {
            var fileNames = Directory.GetFiles(_directoryPath, "*.json", SearchOption.TopDirectoryOnly);

            foreach (var fileName in fileNames)
            {
                var dictionary = CreateJsonLocalizationDictionary(fileName);
                if (Dictionaries.ContainsKey(dictionary.CultureInfo.Name))
                {
                    throw new AbpInitializationException(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.Name);
                }

                Dictionaries[dictionary.CultureInfo.Name] = dictionary;

                if (fileName.EndsWith(sourceName + ".json"))
                {
                    if (DefaultDictionary != null)
                    {
                        throw new AbpInitializationException("Only one default localization dictionary can be for source: " + sourceName);
                    }

                    DefaultDictionary = dictionary;
                }
            }
        }

        /// <summary>
        /// 创建Json本地化字典
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual JsonLocalizationDictionary CreateJsonLocalizationDictionary(string fileName)
        {
            return JsonLocalizationDictionary.BuildFromFile(fileName);
        }
    }
}