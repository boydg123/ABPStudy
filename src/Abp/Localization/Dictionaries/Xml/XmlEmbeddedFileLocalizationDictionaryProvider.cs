using System.Reflection;

namespace Abp.Localization.Dictionaries.Xml
{
    /// <summary>
    /// Provides localization dictionaries from XML files embedded into an <see cref="Assembly"/>.
    /// 从嵌入到程序集的xml文件中提供本地化字典
    /// </summary>
    public class XmlEmbeddedFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
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
        /// Creates a new <see cref="XmlEmbeddedFileLocalizationDictionaryProvider"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="assembly">Assembly that contains embedded xml files / 包含嵌入XML文件的程序集</param>
        /// <param name="rootNamespace">Namespace of the embedded xml dictionary files / 嵌入字典文件的名称空间</param>
        public XmlEmbeddedFileLocalizationDictionaryProvider(Assembly assembly, string rootNamespace)
        {
            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sourceName">源名称</param>
        public override void Initialize(string sourceName)
        {
            var resourceNames = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(_rootNamespace))
                {
                    using (var stream = _assembly.GetManifestResourceStream(resourceName))
                    {
                        var xmlString = Utf8Helper.ReadStringFromStream(stream);

                        var dictionary = CreateXmlLocalizationDictionary(xmlString);
                        if (Dictionaries.ContainsKey(dictionary.CultureInfo.Name))
                        {
                            throw new AbpInitializationException(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.Name);
                        }

                        Dictionaries[dictionary.CultureInfo.Name] = dictionary;

                        if (resourceName.EndsWith(sourceName + ".xml"))
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
        /// 创建XML本地字典
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        protected virtual XmlLocalizationDictionary CreateXmlLocalizationDictionary(string xmlString)
        {
            return XmlLocalizationDictionary.BuildFomXmlString(xmlString);
        }
    }
}