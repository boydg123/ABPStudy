using System.Threading.Tasks;

namespace Abp.Web.Authorization
{
    /// <summary>
    /// This class is used to build authorization script.
    /// 此类用于构建授权脚本
    /// </summary>
    public interface IAuthorizationScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains all authorization information.
        /// 获取包含所有授权信息的Javascript脚本
        /// </summary>
        Task<string> GetScriptAsync();
    }
}
