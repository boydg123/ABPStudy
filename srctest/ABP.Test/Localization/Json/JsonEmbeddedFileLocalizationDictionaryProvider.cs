using Xunit;
using Shouldly;
using Abp.Localization.Dictionaries.Json;
using System.Linq;
using System.Reflection;


namespace ABP.Test.Localization.Json
{
    /// <summary>
    /// 嵌入到程序集中Json文件提供本地化字符串的测试
    /// </summary>
    public class JsonEmbeddedFileLocalizationDictionaryProvider_Test
    {
        private readonly JsonEmbeddedFileLocalizationDictionaryProvider _dictionaryProvider;

        public JsonEmbeddedFileLocalizationDictionaryProvider_Test()
        {
            _dictionaryProvider = new JsonEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(), "ABP.Test.Localization.Json.JsonSources"
                );

            _dictionaryProvider.Initialize("Lang");
        }

        /// <summary>
        /// 本地化字符串字典集合测试
        /// </summary>
        [Fact]
        public void Should_Get_Dictionaries()
        {
            //获取集合的值
            var dictionaries = _dictionaryProvider.Dictionaries.Values.ToList();

            //两个区域信息 "en" 和 "zh-CN"
            dictionaries.Count.ShouldBe(2);

            var enDict = dictionaries.FirstOrDefault(d => d.CultureInfo.Name == "en");
            enDict.ShouldNotBe(null);
            enDict["Apple"].ShouldBe("Apple");
            enDict["Banana"].ShouldBe("Banana");

            var zhCNDict = dictionaries.FirstOrDefault(d => d.CultureInfo.Name == "zh-CN");
            zhCNDict.ShouldNotBe(null);
            zhCNDict["Apple"].ShouldBe("苹果");
            zhCNDict["Banana"].ShouldBe("香蕉");
        }
    }
}
