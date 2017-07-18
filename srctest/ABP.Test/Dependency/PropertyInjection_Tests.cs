using Abp.Application.Services;
using Xunit;
using Abp.Dependency;
using Shouldly;
using System;
using NSubstitute;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// 属性注入测试
    /// </summary>
    public class PropertyInjection_Tests : TestBaseWithLocalManager
    {
        private class MyApplicationService : ApplicationService
        {
            public void TestSession()
            {
                //AbpSession不是具体实现，而是代理
                AbpSession.ShouldNotBe(null);
                AbpSession.TenantId.ShouldBe(1);
                AbpSession.UserId.ShouldBe(42);
            }
        }

        /// <summary>
        /// 为ApplicationService注入Session值
        /// </summary>
        [Fact]
        public void Should_Inject_Session_For_ApplicationService()
        {
            var session = Substitute.For<IAbpSession>();//session为IAbpSession创建时的代替实例
            var a = session.TenantId.Returns(1);//调用属性返回属性值1
            session.UserId.Returns(42);


            localIocManager.IocContainer.Register(
                Component.For<IAbpSession>().UsingFactoryMethod(() => session)
                );
            localIocManager.Register<MyApplicationService>();

            var myAppService = localIocManager.Resolve<MyApplicationService>();
            myAppService.TestSession();
        }
    }
}
