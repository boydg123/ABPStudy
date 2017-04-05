using Abp.Application.Services;
using Abp.Aspects;
using Abp.Dependency;
using Castle.DynamicProxy;

namespace Abp.Runtime.Validation.Interception
{
    /// <summary>
    /// This interceptor is used intercept method calls for classes which's methods must be validated.
    /// 此拦截器用于拦截调用时需要验证的方法
    /// </summary>
    public class ValidationInterceptor : IInterceptor
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver"></param>
        public ValidationInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.InvocationTarget, AbpCrossCuttingConcerns.Validation))
            {
                invocation.Proceed();
                return;
            }

            using (var validator = _iocResolver.ResolveAsDisposable<MethodInvocationValidator>())
            {
                validator.Object.Initialize(invocation.Method, invocation.Arguments);
                validator.Object.Validate();
            }
            
            invocation.Proceed();
        }
    }
}
