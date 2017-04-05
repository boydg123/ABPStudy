using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// 授权帮助接口
    /// </summary>
    public interface IAuthorizationHelper
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="authorizeAttributes">ABP授权特性</param>
        /// <returns></returns>
        Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        Task AuthorizeAsync(MethodInfo methodInfo);
    }
}