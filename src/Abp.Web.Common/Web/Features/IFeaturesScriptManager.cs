using System.Threading.Tasks;

namespace Abp.Web.Features
{
    /// <summary>
    /// This class is used to build feature system script.
    /// 此类用于构建功能系统脚本
    /// </summary>
    public interface IFeaturesScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains all feature information.
        /// 获取包含所有功能信息的Javascript
        /// </summary>
        Task<string> GetScriptAsync();
    }
}