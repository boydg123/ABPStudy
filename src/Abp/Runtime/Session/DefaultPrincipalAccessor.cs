using System.Security.Claims;
using System.Threading;
using Abp.Dependency;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// <see cref="IPrincipalAccessor"/>的默认实现
    /// </summary>
    public class DefaultPrincipalAccessor : IPrincipalAccessor, ISingletonDependency
    {
        /// <summary>
        /// 系统是否支持多个基于声明的身份
        /// </summary>
        public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;

        /// <summary>
        /// 构造函数
        /// </summary>
        public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();
    }
}