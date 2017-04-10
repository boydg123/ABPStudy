namespace Abp.Web.MultiTenancy
{
    /// <summary>
    /// Used to create client scripts for multi-tenancy.
    /// 多租户脚本管理器(用于为多租户创建客户端脚本)
    /// </summary>
    public interface IMultiTenancyScriptManager
    {
        /// <summary>
        /// 获取多租户客户端脚本
        /// </summary>
        /// <returns></returns>
        string GetScript();
    }
}