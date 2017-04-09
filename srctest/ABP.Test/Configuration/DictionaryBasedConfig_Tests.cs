using Xunit;
using Shouldly;
using Abp.Configuration;

namespace ABP.Test.Configuration
{
    /// <summary>
    /// 自定义配置测试
    /// </summary>
    public class DictionaryBasedConfig_Tests
    {
        private readonly MyConfig _config;
        public DictionaryBasedConfig_Tests()
        {
            _config = new MyConfig();
        }

        /// <summary>
        /// 获取设置值测试
        /// </summary>
        [Fact]
        public void Should_Get_Value()
        {
            var testObject = new TestClass() { Value = 1 };

            _config["IntValue"] = 1;
            _config["StringValue"] = "Test String";
            _config["ObjectValue"] = testObject;

            _config["IntValue"].ShouldBe(1);
            _config.Get<int>("IntValue").ShouldBe(1);

            _config["StringValue"].ShouldBe("Test String");
            _config.Get<string>("StringValue").ShouldBe("Test String");

            _config["ObjectValue"].ShouldBeSameAs(testObject);
            _config.Get<TestClass>("ObjectValue").ShouldBeSameAs(testObject);
            _config.Get<TestClass>("ObjectValue").Value.ShouldBe(1);
        }

        /// <summary>
        /// 如果没有摄设置值则返回默认值
        /// </summary>
        [Fact]
        public void Should_Get_Default_If_No_Value()
        {
            _config["MyUndefinedName"].ShouldBe(null);
            _config.Get<string>("MyUndefinedName").ShouldBe(null);
            _config.Get<MyConfig>("MyUndefinedName").ShouldBe(null);
            _config.Get<int>("MyUndefinedName").ShouldBe(0);
        }
        private class MyConfig : DictionaryBasedConfig { }
        private class TestClass { public int Value { get; set; } }
    }
}
