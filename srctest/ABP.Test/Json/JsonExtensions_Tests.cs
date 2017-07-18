using Xunit;
using Shouldly;
using Abp.Json;

namespace Abp.Test.Json
{
    public class JsonExtensions_Tests
    {
        [Fact]
        public void ToJsonString_Test()
        {
            1.ToJsonString().ShouldBe("1");
        }
    }
}
