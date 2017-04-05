using Abp.Dependency;
using Xunit;
using Shouldly;
using Castle.MicroKernel;

namespace ABP.Test.Dependency
{
    /// <summary>
    /// 循环构造函数依赖测试
    /// </summary>
    public class Circular_Constructor_Dependency_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 循环构造函数依赖
        /// </summary>
        [Fact]
        public void Should_Fail_Circular_Constructor_Dependency()
        {
            localIocManager.Register<MyClass1>();
            localIocManager.Register<MyClass2>();
            localIocManager.Register<MyClass3>();

            Assert.Throws<CircularDependencyException>(() => localIocManager.Resolve<MyClass1>());
        }
        public class MyClass1
        {
            public MyClass1(MyClass2 obj)
            {
            }
        }
        public class MyClass2
        {
            public MyClass2(MyClass3 obj)
            {
            }
        }
        public class MyClass3
        {
            public MyClass3(MyClass1 obj)
            {
            }
        }
    }
}
