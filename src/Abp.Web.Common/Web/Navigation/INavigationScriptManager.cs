using System.Threading.Tasks;

namespace Abp.Web.Navigation
{
    /// <summary>
    /// Used to generate navigation scripts.
    /// 导航脚本管理器(用于生成导航脚本)
    /// </summary>
    public interface INavigationScriptManager
    {
        /// <summary>
        /// Used to generate navigation scripts.
        /// 用于生成导航脚本
        /// </summary>
        /// <returns></returns>
        Task<string> GetScriptAsync();
    }
}
