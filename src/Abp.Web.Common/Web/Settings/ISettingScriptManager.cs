using System.Threading.Tasks;

namespace Abp.Web.Settings
{
    /// <summary>
    /// Define interface to get setting scripts
    /// 定义设置脚本的接口
    /// </summary>
    public interface ISettingScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains setting values.
        /// 获取包含设置值的Javascript脚本
        /// </summary>
        Task<string> GetScriptAsync();
    }
}