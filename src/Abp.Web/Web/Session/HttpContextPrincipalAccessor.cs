using System.Security.Claims;
using System.Web;
using Abp.Runtime.Session;

namespace Abp.Web.Session
{
    /// <summary>
    /// <see cref="IPrincipalAccessor"/>的Http上下文实现(主要的访问)
    /// </summary>
    public class HttpContextPrincipalAccessor : DefaultPrincipalAccessor
    {
        /// <summary>
        /// 系统是否支持多个基于声明的身份. Http上下文中当前用户
        /// </summary>
        public override ClaimsPrincipal Principal => HttpContext.Current?.User as ClaimsPrincipal ?? base.Principal;
    }
}
