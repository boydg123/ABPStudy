using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    /// <see cref="AuthorizationHelper"/>的扩展
    /// </summary>
    public static class AuthorizationHelperExtensions
    {
        /// <summary>
        /// 授权 - 异步
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类接口</param>
        /// <param name="authorizeAttribute">ABP授权特性</param>
        /// <returns></returns>
        public static async Task AuthorizeAsync(this IAuthorizationHelper authorizationHelper, IAbpAuthorizeAttribute authorizeAttribute)
        {
            await authorizationHelper.AuthorizeAsync(new[] { authorizeAttribute });
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类接口</param>
        /// <param name="authorizeAttributes">可迭代的ABP授权特性列表</param>
        public static void Authorize(this IAuthorizationHelper authorizationHelper, IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(authorizeAttributes));
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类接口</param>
        /// <param name="authorizeAttribute">ABP授权特性</param>
        public static void Authorize(this IAuthorizationHelper authorizationHelper, IAbpAuthorizeAttribute authorizeAttribute)
        {
            authorizationHelper.Authorize(new[] { authorizeAttribute });
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类接口</param>
        /// <param name="methodInfo">方法信息</param>
        public static void Authorize(this IAuthorizationHelper authorizationHelper, MethodInfo methodInfo)
        {
            AsyncHelper.RunSync(() => authorizationHelper.AuthorizeAsync(methodInfo));
        }
    }
}