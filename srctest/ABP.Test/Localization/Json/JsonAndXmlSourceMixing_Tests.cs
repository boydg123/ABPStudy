using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Xunit;
using Shouldly;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Abp.Test.Localization.Json
{
    /// <summary>
    /// Json和Xml Source混合测试
    /// </summary>
    public class JsonAndXmlSourceMixing_Tests : TestBaseWithLocalManager
    {
        private readonly AbpBootstrapper _bootStrapper;
        public JsonAndXmlSourceMixing_Tests()
        {
            localIocManager.Register<ILanguageManager, LanguageManager>();
            localIocManager.Register<ILanguageProvider, DefaultLanguageProvider>();

            _bootStrapper = AbpBootstrapper.Create<MyLangModule>(localIocManager);
            _bootStrapper.Initialize();
        }

        /// <summary>
        /// 测试Xml和Json.如果在Json和Xml中设置同一个Key，Json貌似 > Xml
        /// </summary>
        [Fact]
        public void Test_Xml_Json()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            var manager = localIocManager.Resolve<LocalizationManager>();

            var source = manager.GetSource("Lang");

            source.GetString("Apple").ShouldBe("Apple");
            source.GetString("Name").ShouldBe("Derrick");
            source.GetString("name1").ShouldBe("name");

            var allStr = source.GetAllStrings();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");

            var str = source.GetString("Apple");
            var all = source.GetAllStrings();
            source.GetString("Apple").ShouldBe("苹果");
        }
    }

    /// <summary>
    /// Lang模块
    /// </summary>
    public class MyLangModule : AbpModule
    {
        public override void PreInitialize()
        {

            Configuration.Localization.Sources.Extensions.Add(
               new LocalizationSourceExtensionInfo("Lang", new JsonEmbeddedFileLocalizationDictionaryProvider(
                  Assembly.GetExecutingAssembly(), "Abp.Test.Localization.Json.JsonSources"))
               );

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource("Lang", new XmlEmbeddedFileLocalizationDictionaryProvider(
                    Assembly.GetExecutingAssembly(), "Abp.Test.Localization.Json.XmlSources"))
                    );

           
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
