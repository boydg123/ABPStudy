using System;
using System.Reflection;

namespace Abp.Aspects
{
    //THIS NAMESPACE IS WORK-IN-PROGRESS

    /// <summary>
    /// 表示切面（AOP)属性
    /// </summary>
    internal abstract class AspectAttribute : Attribute
    {
        /// <summary>
        /// 拦截器类型
        /// </summary>
        public Type InterceptorType { get; set; }

        /// <summary>
        /// 创建一个<see cref="AspectAttribute"/>对象
        /// </summary>
        /// <param name="interceptorType"></param>
        protected AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }
    }

    /// <summary>
    /// Abp拦截上下文
    /// </summary>
    internal interface IAbpInterceptionContext
    {
        /// <summary>
        /// 目标
        /// </summary>
        object Target { get; }

        /// <summary>
        /// 方法
        /// </summary>
        MethodInfo Method { get; }

        /// <summary>
        /// 参数
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// 返回值
        /// </summary>
        object ReturnValue { get; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        bool Handled { get; set; }
    }

    /// <summary>
    /// Abp异常（发生前）拦截上下文
    /// </summary>
    internal interface IAbpBeforeExecutionInterceptionContext : IAbpInterceptionContext
    {

    }

    /// <summary>
    /// Abp异常（发生后）拦截上下文
    /// </summary>
    internal interface IAbpAfterExecutionInterceptionContext : IAbpInterceptionContext
    {
        Exception Exception { get; }
    }

    /// <summary>
    /// ABP系统拦截器接口
    /// </summary>
    /// <typeparam name="TAspect"></typeparam>
    internal interface IAbpInterceptor<TAspect>
    {
        /// <summary>
        /// 拦截类
        /// </summary>
        TAspect Aspect { get; set; }

        /// <summary>
        /// Abp异常（发生前）拦截
        /// </summary>
        /// <param name="context"></param>
        void BeforeExecution(IAbpBeforeExecutionInterceptionContext context);

        /// <summary>
        /// Abp异常（发生后）拦截
        /// </summary>
        /// <param name="context"></param>
        void AfterExecution(IAbpAfterExecutionInterceptionContext context);
    }

    /// <summary>
    /// ABP系统拦截器基类
    /// </summary>
    /// <typeparam name="TAspect">拦截类</typeparam>
    internal abstract class AbpInterceptorBase<TAspect> : IAbpInterceptor<TAspect>
    {
        /// <summary>
        /// 拦截类
        /// </summary>
        public TAspect Aspect { get; set; }

        /// <summary>
        /// Abp异常（发生前）拦截
        /// </summary>
        /// <param name="context"></param>
        public virtual void BeforeExecution(IAbpBeforeExecutionInterceptionContext context)
        {
        }

        /// <summary>
        /// Abp异常（发生后）拦截
        /// </summary>
        /// <param name="context"></param>
        public virtual void AfterExecution(IAbpAfterExecutionInterceptionContext context)
        {
        }
    }

    /// <summary>
    /// 拦截测试
    /// </summary>
    internal class Test_Aspects
    {
        internal class MyAspectAttribute : AspectAttribute
        {
            public int TestValue { get; set; }

            public MyAspectAttribute()
                : base(typeof(MyInterceptor))
            {
            }
        }

        internal class MyInterceptor : AbpInterceptorBase<MyAspectAttribute>
        {
            public override void BeforeExecution(IAbpBeforeExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }

            public override void AfterExecution(IAbpAfterExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }
        }

        public class MyService
        {
            [MyAspect(TestValue = 41)] //Usage!
            public void DoIt()
            {

            }
        }

        public class MyClient
        {
            private readonly MyService _service;

            public MyClient(MyService service)
            {
                _service = service;
            }

            public void Test()
            {
                _service.DoIt();
            }
        }
    }
}
