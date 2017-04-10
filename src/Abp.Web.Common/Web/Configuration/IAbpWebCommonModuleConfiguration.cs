using Abp.Web.Api.ProxyScripting.Configuration;
using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// Used to configure ABP Web Common module.
    /// 用于配置ABP Web Common模块
    /// </summary>
    public interface IAbpWebCommonModuleConfiguration
    {
        /// <summary>
        /// If this is set to true, all exception and details are sent directly to clients on an error.
        /// 如果此属性设置为true，所有异常和详细信息都直接发送到客户端上的错误
        /// Default: false (ABP hides exception details from clients except special exceptions.)
        /// 默认值：false(ABP在客户端隐藏异常详情，指定的异常除外)
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }

        /// <summary>
        /// Used to configure Api proxy scripting.
        /// 用于配置Api代理脚本
        /// </summary>
        IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        /// <summary>
        /// Used to configure Anti Forgery security settings.
        /// 用于配置防伪安全设置
        /// </summary>
        IAbpAntiForgeryConfiguration AntiForgery { get; }
    }
}