using System.Security.Claims;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// 主要的访问
    /// </summary>
    public interface IPrincipalAccessor
    {
        /// <summary>
        /// 系统是否支持多个基于声明的身份
        /// </summary>
        ClaimsPrincipal Principal { get; }
    }
}
