using Abp.Web.Api.ProxyScripting.Configuration;
using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// 用于配置ABP Web Common模块
    /// </summary>
    internal class AbpWebCommonModuleConfiguration : IAbpWebCommonModuleConfiguration
    {
        /// <summary>
        /// 如果此属性设置为true，所有异常和详细信息都直接发送到客户端上的错误.
        /// 默认值：false(ABP在客户端隐藏异常详情，指定的异常除外)
        /// </summary>
        public bool SendAllExceptionsToClients { get; set; }

        /// <summary>
        /// 用于配置Api代理脚本
        /// </summary>
        public IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        /// <summary>
        /// 用于配置防伪安全设置
        /// </summary>
        public IAbpAntiForgeryConfiguration AntiForgery { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiProxyScripting">用于配置Api代理脚本</param>
        /// <param name="abpAntiForgery">用于配置防伪安全设置</param>
        public AbpWebCommonModuleConfiguration(IApiProxyScriptingConfiguration apiProxyScripting, IAbpAntiForgeryConfiguration abpAntiForgery)
        {
            ApiProxyScripting = apiProxyScripting;
            AntiForgery = abpAntiForgery;
        }
    }
}