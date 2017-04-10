using Abp.Web.Api.Modeling;

namespace Abp.Web.Api.ProxyScripting.Generators
{
    /// <summary>
    /// 代理脚本生成器
    /// </summary>
    public interface IProxyScriptGenerator
    {
        /// <summary>
        /// 创建脚本
        /// </summary>
        /// <param name="model">API应用程序描述模型</param>
        /// <returns></returns>
        string CreateScript(ApplicationApiDescriptionModel model);
    }
}