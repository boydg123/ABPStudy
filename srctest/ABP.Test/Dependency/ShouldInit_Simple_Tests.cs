using Abp;
using Abp.Dependency;
using Shouldly;
using Xunit;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 验证类自动创建的时候是否自动调用初始化方法
    /// </summary>
    public class ShouldInit_Simple_Tests : TestBaseWithLocalManager
    {
        [Fact]
        public void Should_Call_Init()
        {
            localIocManager.Register<MyService>(DependencyLifeStyle.Transient);
            var myService = localIocManager.Resolve<MyService>();
            var myService2 = localIocManager.Resolve<MyService>();
            myService.InitCount.ShouldBe(1);
            myService2.InitCount.ShouldBe(1);
            //localIocManager.Register<MyService>(DependencyLifeStyle.Singleton);
            //var myService3 = localIocManager.Resolve<MyService>();
            //var myService4 = localIocManager.Resolve<MyService>();
            //myService3.InitCount.ShouldBe(1);
            //myService4.InitCount.ShouldBe(2);
        }

        /// <summary>
        /// 在对象创建后自动调用初始化方法
        /// </summary>
        public class MyService : IShouldInitialize
        {
            public int InitCount { set; get; }
            public void Initialize()
            {
                InitCount++;
            }
        }
    }
}
