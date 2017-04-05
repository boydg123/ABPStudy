using Castle.DynamicProxy;

namespace Abp.Authorization
{
    /// <summary>
    /// This class is used to intercept methods to make authorization if the method defined <see cref="AbpAuthorizeAttribute"/>.
    /// 如果方法使用了特性<see cref="AbpAuthorizeAttribute"/>,此类用于拦截方法，作授权验证
    /// </summary>
    public class AuthorizationInterceptor : IInterceptor
    {
        /// <summary>
        /// 授权帮助类接口
        /// </summary>
        private readonly IAuthorizationHelper _authorizationHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类接口</param>
        public AuthorizationInterceptor(IAuthorizationHelper authorizationHelper)
        {
            _authorizationHelper = authorizationHelper;
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            _authorizationHelper.Authorize(invocation.MethodInvocationTarget);
            invocation.Proceed();
        }
    }
}
