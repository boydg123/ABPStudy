namespace Abp.Web.Security
{
    /// <summary>
    /// 安全脚本管理器
    /// </summary>
    public interface ISecurityScriptManager
    {
        /// <summary>
        /// 获取安全脚本
        /// </summary>
        /// <returns></returns>
        string GetScript();
    }
}