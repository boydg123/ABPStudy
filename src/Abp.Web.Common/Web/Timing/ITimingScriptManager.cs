using System.Threading.Tasks;

namespace Abp.Web.Timing
{
    /// <summary>
    /// Define interface to get timing scripts
    /// 定义获取时间脚本的接口
    /// </summary>
    public interface ITimingScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains all feature information.
        /// 获取包含所有功能信息的Javascript脚本
        /// </summary>
        Task<string> GetScriptAsync();
    }
}
