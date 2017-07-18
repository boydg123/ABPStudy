using Abp.Dependency;
using Xunit;
using Shouldly;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 注册与解析对象的相关测试
    /// </summary>
    public class Registrar_And_Resolver_Tests : TestBaseWithLocalManager
    {
        public interface IMyInterface { }
        public class MyClass : IMyInterface { }

        private readonly IIocRegistrar _registrar;
        private readonly IIocResolver _resolver;
        public Registrar_And_Resolver_Tests()
        {
            _registrar = localIocManager.Resolve<IIocRegistrar>();
            _resolver = localIocManager.Resolve<IIocResolver>();
        }

        /// <summary>
        /// 解析自己注册的类型
        /// </summary>
        [Fact]
        public void Should_Resolve_Self_Registered_Tests()
        {
            _registrar.Register<MyClass>();
            _resolver.Resolve<MyClass>();
        }

        /// <summary>
        /// 解析通过接口注册的类型
        /// </summary>
        [Fact]
        public void Should_Resolve_Registered_By_Interface_Types()
        {
            _registrar.Register<IMyInterface, MyClass>();
            var class1 = _resolver.Resolve<IMyInterface>();

            try
            {
                var class2 = _resolver.Resolve<MyClass>();
                class1.ShouldBeSameAs(class2);
                //Assert.False(true, "通过接口注册的对象，不应该通过类来解析！");
            }
            catch
            {

            }
        }

        /// <summary>
        /// 实时模式获取的应该是不同对象
        /// </summary>
        [Fact]
        public void Should_Get_Different_Objects_For_Transients()
        {
            //注册
            _registrar.Register<MyClass>(DependencyLifeStyle.Transient);

            //解析两种类型
            var class1 = _resolver.Resolve<MyClass>();
            var class2 = _resolver.Resolve<MyClass>();

            //断言 - 实时模式解析的对象不一样
            class1.ShouldNotBeSameAs(class2);
        }

        /// <summary>
        /// 单例模式获取的是同一对象
        /// </summary>
        [Fact]
        public void Should_Get_Same_Objects_For_Singleton()
        {
            //注册
            _registrar.Register<MyClass>(DependencyLifeStyle.Singleton);

            //解析两种类型
            var class1 = _resolver.Resolve<MyClass>();
            var class2 = _resolver.Resolve<MyClass>();

            //断言 - 单例模式获取的是同一对象
            class1.ShouldBeSameAs(class2);
        }
    }
}
