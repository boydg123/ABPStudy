using System.Reflection;
using Abp.Localization.Dictionaries.Xml;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    /// Provides localization dictionaries from JSON files embedded into an <see cref="Assembly"/>.
    /// 从嵌入到程序集的JSON文件中提供本地化字典
    /// </summary>
    public class JsonEmbeddedFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
    {
        /// <summary>
        /// 程序集
        /// </summary>
        private readonly Assembly _assembly;

        /// <summary>
        /// 根名称空间
        /// </summary>
        private readonly string _rootNamespace;

        /// <summary>
        /// Creates a new <see cref="JsonEmbeddedFileLocalizationDictionaryProvider"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="assembly">Assembly that contains embedded json files / 包含嵌入json文件的程序集</param>
        /// <param name="rootNamespace">
        /// <para>
        /// Namespace of the embedded json dictionary files
        /// 内嵌JSON字典文件的命名空间
        /// </para>
        /// <para>
        /// Notice : Json folder name is different from Xml folder name.
        /// 提示：Json文件夹名称不同于Xml文件夹名称
        /// </para>
        /// <para>
        /// You must name it like this : Json**** and Xml****; Do not name : ****Json and ****Xml
        /// 你必须命名它像:Json**** and Xml****; Do not name : ****Json and ****Xml
        /// </para>
        /// </param>
        public JsonEmbeddedFileLocalizationDictionaryProvider(Assembly assembly, string rootNamespace)
        {
            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName"></param>
        public override void Initialize(string sourceName)
        {
            var resourceNames = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(_rootNamespace))
                {
                    using (var stream = _assembly.GetManifestResourceStream(resourceName))
                    {
                        var jsonString = Utf8Helper.ReadStringFromStream(stream);

                        var dictionary = CreateJsonLocalizationDictionary(jsonString);
                        if (Dictionaries.ContainsKey(dictionary.CultureInfo.Name))
                        {
                            throw new AbpInitializationException(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.Name);
                        }

                        Dictionaries[dictionary.CultureInfo.Name] = dictionary;

                        if (resourceName.EndsWith(sourceName + ".json"))
                        {
                            if (DefaultDictionary != null)
                            {
                                throw new AbpInitializationException("Only one default localization dictionary can be for source: " + sourceName);
                            }

                            DefaultDictionary = dictionary;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建Json本地化字典
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        protected virtual JsonLocalizationDictionary CreateJsonLocalizationDictionary(string jsonString)
        {
            return JsonLocalizationDictionary.BuildFromJsonString(jsonString);
        }
    }
}