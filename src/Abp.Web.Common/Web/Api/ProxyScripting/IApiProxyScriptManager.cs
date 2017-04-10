namespace Abp.Web.Api.ProxyScripting
{
    /// <summary>
    /// API代理脚本管理器
    /// </summary>
    public interface IApiProxyScriptManager
    {
        /// <summary>
        /// 获取API代理脚本
        /// </summary>
        /// <param name="options">API代理生成选项</param>
        /// <returns></returns>
        string GetScript(ApiProxyGenerationOptions options);
    }
}