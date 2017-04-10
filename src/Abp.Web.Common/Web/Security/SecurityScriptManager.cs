using System.Text;
using Abp.Dependency;
using Abp.Web.Security.AntiForgery;

namespace Abp.Web.Security
{
    /// <summary>
    /// 安全脚本管理器默认实现
    /// </summary>
    internal class SecurityScriptManager : ISecurityScriptManager, ITransientDependency
    {
        /// <summary>
        /// ABP防伪配置
        /// </summary>
        private readonly IAbpAntiForgeryConfiguration _abpAntiForgeryConfiguration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpAntiForgeryConfiguration">ABP防伪配置</param>
        public SecurityScriptManager(IAbpAntiForgeryConfiguration abpAntiForgeryConfiguration)
        {
            _abpAntiForgeryConfiguration = abpAntiForgeryConfiguration;
        }

        /// <summary>
        /// 生成安全脚本
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.security.antiForgery.tokenCookieName = '" + _abpAntiForgeryConfiguration.TokenCookieName + "';");
            script.AppendLine("    abp.security.antiForgery.tokenHeaderName = '" + _abpAntiForgeryConfiguration.TokenHeaderName + "';");
            script.Append("})();");

            return script.ToString();
        }
    }
}
