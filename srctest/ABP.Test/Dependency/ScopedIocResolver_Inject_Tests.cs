using Abp.Dependency;
using Xunit;
using Shouldly;
using System;

namespace ABP.Test.Dependency
{
    public class ScopedIocResolver_Inject_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 当注入类释放时，应该自动释放解析依赖项
        /// </summary>
        [Fact]
        public void Should_Automatically_Release_Resolved_Dependencies_When_Injected_Class_Released()
        {
            //注册对象
            localIocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);
            localIocManager.Register<MyDependency>(DependencyLifeStyle.Transient);
            localIocManager.Register<MyMainClass>(DependencyLifeStyle.Transient);
            //获取对象
            var mainClass = localIocManager.Resolve<MyMainClass>();
            var dependency = mainClass.CreateMyDependency();

            //断言
            dependency.IsDisposed.ShouldBe(false);
            localIocManager.Release(mainClass); //在Release之前会调用Dispose方法
            dependency.IsDisposed.ShouldBe(true);
        }

        /// <summary>
        /// 依赖类
        /// </summary>
        public class MyDependency : IDisposable
        {
            public bool IsDisposed { get; set; }
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        /// <summary>
        /// 主类 - 依赖 <see cref="MyDependency"/>
        /// </summary>
        public class MyMainClass
        {
            private readonly IScopedIocResolver _resolver;

            public MyMainClass(IScopedIocResolver resolver)
            {
                _resolver = resolver;
            }

            public MyDependency CreateMyDependency()
            {
                return _resolver.Resolve<MyDependency>();
            }
        }
    }
}
