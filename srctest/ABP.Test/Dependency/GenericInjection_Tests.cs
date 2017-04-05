using Abp.Dependency;
using Xunit;
using Shouldly;
using System;
using Castle.MicroKernel.Registration;

namespace ABP.Test.Dependency
{
    /// <summary>
    /// 泛型注入测试
    /// </summary>
    public class GenericInjection_Tests : TestBaseWithLocalManager
    {
        public class MyClass { }
        public interface IGenericInterface<T> where T : class
        {
            T GenericArg { get; set; }
        }

        public class GenericImplClass<T> : IGenericInterface<T> where T : class
        {
            public T GenericArg { get; set; }
        }

        /// <summary>
        /// 泛型类型注册解析
        /// </summary>
        [Fact]
        public void Should_Resolve_Generic_Types()
        {
            //localIocManager.IocContainer.Register(
            //    Component.For<MyClass>(),
            //    Component.For(typeof(IGenericInterface<>)).ImplementedBy(typeof(GenericImplClass<>))
            //    );

            localIocManager.Register<MyClass>(DependencyLifeStyle.Transient);
            localIocManager.Register(typeof(IGenericInterface<>), typeof(GenericImplClass<>), DependencyLifeStyle.Transient);

            var genericObj = localIocManager.Resolve<IGenericInterface<MyClass>>();
            genericObj.GenericArg.GetType().ShouldBe(typeof(MyClass));
        }
    }
}
