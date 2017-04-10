namespace Abp.Web.Sessions
{
    /// <summary>
    /// Used to create client scripts for session.
    /// 用于为Session创建客户端脚本
    /// </summary>
    public interface ISessionScriptManager
    {
        /// <summary>
        /// 获取Session客户端脚本
        /// </summary>
        /// <returns></returns>
        string GetScript();
    }
}
