using Abp.Dependency;
using Xunit;
using Shouldly;
using System.Linq;
using Castle.MicroKernel.Registration;

namespace ABP.Test.Dependency
{
    /// <summary>
    /// IOC管理器管理同一接口不同实现对象，关于覆盖的测试
    /// </summary>
    public class IocManager_Override_Tests : TestBaseWithLocalManager
    {
        public interface IEmptyService
        { }
        public class EmptyImpl1 : IEmptyService
        { }
        public class EmptyImpl2 : IEmptyService
        { }
        public class EmptyImpl3 : IEmptyService
        { }

        /// <summary>
        /// 默认情况下是不覆盖的，获取的也是第一个注册的对象
        /// </summary>
        [Fact]
        public void Should_Not_Override_As_Default()
        {
            //注册对象
            localIocManager.Register<IEmptyService, EmptyImpl1>(DependencyLifeStyle.Transient);
            localIocManager.Register<IEmptyService, EmptyImpl2>(DependencyLifeStyle.Transient);
            localIocManager.Register<IEmptyService, EmptyImpl3>(DependencyLifeStyle.Transient);

            //解析对象
            var service = localIocManager.Resolve<IEmptyService>(); //默认是EmptyImpl1
            var allServices = localIocManager.ResolveAll<IEmptyService>();//获取所有实现对象

            //断言
            service.ShouldBeOfType<EmptyImpl1>();
            allServices.Length.ShouldBe(3);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl1)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl2)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl3)).ShouldBe(true);
        }

        /// <summary>
        /// 如果用了IsDefault则被覆盖
        /// </summary>
        [Fact]
        public void Should_Override_When_Using_IsDefault()
        {
            //注册对象
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl1>().LifestyleTransient());
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl2>().LifestyleTransient().IsDefault());

            //解析对象
            var service = localIocManager.Resolve<IEmptyService>(); //默认是EmptyImpl2
            var allServices = localIocManager.IocContainer.ResolveAll<IEmptyService>();//获取所有实现对象

            //断言
            service.ShouldBeOfType<EmptyImpl2>();
            allServices.Length.ShouldBe(2);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl1)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl2)).ShouldBe(true);
        }

        /// <summary>
        /// 如果IsDefault使用两次，则获取的是最后一次使用的对象
        /// </summary>
        [Fact]
        public void Should_Override_When_Using_IsDefault_Twice()
        {
            //注册对象
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl1>().LifestyleTransient());
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl2>().LifestyleTransient().IsDefault());
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl3>().LifestyleTransient().IsDefault());

            //解析对象
            var service = localIocManager.Resolve<IEmptyService>(); //默认是EmptyImpl3
            var allServices = localIocManager.IocContainer.ResolveAll<IEmptyService>();//获取所有实现对象

            //断言
            service.ShouldBeOfType<EmptyImpl3>();
            allServices.Length.ShouldBe(3);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl1)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl2)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl3)).ShouldBe(true);
        }

        /// <summary>
        /// 应获取使用IsDefault的对象
        /// </summary>
        [Fact]
        public void Should_Get_Default_Service()
        {
            //注册对象
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl1>().LifestyleTransient());
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl2>().LifestyleTransient().IsDefault());
            localIocManager.IocContainer.Register(Component.For<IEmptyService>().ImplementedBy<EmptyImpl3>().LifestyleTransient());

            //解析对象
            var service = localIocManager.Resolve<IEmptyService>(); //默认是EmptyImpl2
            var allServices = localIocManager.IocContainer.ResolveAll<IEmptyService>();//获取所有实现对象

            //断言
            service.ShouldBeOfType<EmptyImpl2>();
            allServices.Length.ShouldBe(3);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl1)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl2)).ShouldBe(true);
            allServices.Any(a => a.GetType() == typeof(EmptyImpl3)).ShouldBe(true);
        }
    }
}
