using Xunit;
using Shouldly;
using Castle.MicroKernel.Registration;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// IOC管理器生命周期测试
    /// </summary>
    public class IocManager_LifeStyle_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 实时注入，当对象释放的时候应该调用释放方法
        /// </summary>
        [Fact]
        public void Should_Call_Dispose_Of_Transient_Dependancy_When_Object_Is_Released()
        {
            //IWindsorContainer容器注册类型
            localIocManager.IocContainer.Register(Component.For<SimpleDisposableObject>().LifestyleTransient());
            //从IWindsorContainer容器获取对象，对象是实时模式
            var obj = localIocManager.IocContainer.Resolve<SimpleDisposableObject>();
            //调用对象Release方法。会自动调用Dispose方法。
            localIocManager.IocContainer.Release(obj);
            obj.DisposeCount.ShouldBe(1);
        }

        /// <summary>
        /// 实时注入，当IOC管理器销毁的时候，自动调用释放方法。
        /// </summary>
        [Fact]
        public void Should_Call_Dispose_Of_Transient_Dependancy_When_IocManager_Is_Dispose()
        {
            localIocManager.IocContainer.Register(Component.For<SimpleDisposableObject>().LifestyleTransient());
            var obj = localIocManager.IocContainer.Resolve<SimpleDisposableObject>();
            //IOC管理器释放资源的时候，自动调用对象的释放方法
            localIocManager.Dispose();
            obj.DisposeCount.ShouldBe(1);
        }

        /// <summary>
        /// 单例注入，当IOC管理器销毁的时候自动调用对象的释放方法
        /// </summary>
        [Fact]
        public void Should_Call_Dispose_Of_Singleton_Dependency_When_IocManager_Is_Dispose()
        {
            localIocManager.IocContainer.Register(Component.For<SimpleDisposableObject>().LifestyleSingleton());
            var obj = localIocManager.IocContainer.Resolve<SimpleDisposableObject>();
            localIocManager.Dispose();
            obj.DisposeCount.ShouldBe(1);
        }
    }
}
