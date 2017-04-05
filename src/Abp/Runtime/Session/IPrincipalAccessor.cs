using System.Security.Claims;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// 主要的访问
    /// </summary>
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
