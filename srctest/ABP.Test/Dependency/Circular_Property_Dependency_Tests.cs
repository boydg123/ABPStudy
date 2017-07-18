using Abp.Dependency;
using Xunit;
using Shouldly;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 循环属性依赖测试
    /// </summary>
    public class Circular_Property_Dependency_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 实时对象，循环依赖属性应该成功
        /// </summary>
        [Fact]
        public void Should_Success_Circular_Property_Injection_Transient()
        {
            Initialize_Test(DependencyLifeStyle.Transient);

            var obj1 = localIocManager.Resolve<MyClass1>();
            obj1.Obj2.ShouldNotBe(null);
            obj1.Obj3.ShouldNotBe(null);
            obj1.Obj2.Obj3.ShouldNotBe(null);

            var obj2 = localIocManager.Resolve<MyClass2>();
            obj2.Obj1.ShouldNotBe(null);
            obj2.Obj3.ShouldNotBe(null);
            obj2.Obj1.Obj3.ShouldNotBe(null);

            MyClass1.CreateCount.ShouldBe(2);
            MyClass2.CreateCount.ShouldBe(2);
            MyClass3.CreateCount.ShouldBe(4);
        }

        /// <summary>
        /// 单例对象，循环依赖属性应该成功
        /// </summary>
        [Fact]
        public void Should_Success_Circular_Property_Injection_Singleton()
        {
            Initialize_Test(DependencyLifeStyle.Singleton);

            var obj1 = localIocManager.Resolve<MyClass1>();
            obj1.Obj2.ShouldNotBe(null);
            obj1.Obj3.ShouldNotBe(null);
            obj1.Obj2.Obj3.ShouldNotBe(null);

            var obj2 = localIocManager.Resolve<MyClass2>();
            obj2.Obj1.ShouldBe(null); //Obj1为null
            obj2.Obj3.ShouldNotBe(null);

            MyClass1.CreateCount.ShouldBe(1);
            MyClass2.CreateCount.ShouldBe(1);
            MyClass3.CreateCount.ShouldBe(1);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lifeStyle"></param>
        private void Initialize_Test(DependencyLifeStyle lifeStyle)
        {
            MyClass1.CreateCount = 0;
            MyClass2.CreateCount = 0;
            MyClass3.CreateCount = 0;

            localIocManager.Register<MyClass1>(lifeStyle);
            localIocManager.Register<MyClass2>(lifeStyle);
            localIocManager.Register<MyClass3>(lifeStyle);
        }
        public class MyClass1
        {
            public static int CreateCount { get; set; }

            public MyClass2 Obj2 { get; set; }

            public MyClass3 Obj3 { get; set; }

            public MyClass1()
            {
                CreateCount++;
            }
        }

        public class MyClass2
        {
            public static int CreateCount { get; set; }

            public MyClass1 Obj1 { get; set; }

            public MyClass3 Obj3 { get; set; }

            public MyClass2()
            {
                CreateCount++;
            }
        }

        public class MyClass3
        {
            public static int CreateCount { get; set; }

            public MyClass3()
            {
                CreateCount++;
            }
        }
    }
}
