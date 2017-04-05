using System;
using Xunit;
using Shouldly;

namespace Test1
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            var a = 1;
            a.ShouldBe(1);
        }
    }
}
