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

namespace ABP.Test.Localization.Json
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
        /// 测试Xml和Json
        /// </summary>
        [Fact]
        public void Test_Xml_Json()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        }
    }

    /// <summary>
    /// Lang模块
    /// </summary>
    public class MyLangModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource("Lang",new XmlEmbeddedFileLocalizationDictionaryProvider(
                    Assembly.GetExecutingAssembly(), "ABP.Test.Localization.Json.XmlSources"))
                    );

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    "Lang", new JsonEmbeddedFileLocalizationDictionaryProvider(
                            Assembly.GetExecutingAssembly(), "ABP.Test.Localization.Json.JsonSources"
                        )
                    )
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
