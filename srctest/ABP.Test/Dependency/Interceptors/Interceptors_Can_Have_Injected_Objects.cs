using Castle.DynamicProxy;
using Xunit;
using Shouldly;
using System;
using Castle.MicroKernel.Registration;

namespace ABP.Test.Dependency.Interceptors
{
    /// <summary>
    /// 拦截器能被注入
    /// </summary>
    public class Interceptors_Can_Have_Injected_Objects : TestBaseWithLocalManager
    {
        /// <summary>
        /// 拦截方法被调用
        /// </summary>
        [Fact]
        public void Interceptors_Should_Work()
        {
            localIocManager.IocContainer.Register(
                Component.For<BracketInterceptor>().LifestyleTransient(),
                Component.For<MyInterceptorClass>().Interceptors<BracketInterceptor>().LifestyleTransient()
                );

            var greetingObj = localIocManager.Resolve<MyInterceptorClass>();

            greetingObj.SayHello("Halil").ShouldBe("(Hello Halil)");
        }

        public class MyInterceptorClass
        {
            public virtual string SayHello(string name)
            {
                return "Hello " + name;
            }
        }
        public class BracketInterceptor : IInterceptor
        {
            /// <summary>
            /// 拦截方法
            /// </summary>
            /// <param name="invocation">封装一个代理方法的调用</param>
            public void Intercept(IInvocation invocation)
            {
                invocation.Proceed();
                invocation.ReturnValue = "(" + invocation.ReturnValue + ")";
            }
        }
    }
}
