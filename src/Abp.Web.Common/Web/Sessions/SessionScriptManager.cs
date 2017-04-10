using System.Text;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Abp.Web.Sessions
{
    /// <summary>
    /// Session脚本管理器(用于为Session创建客户端脚本)
    /// </summary>
    public class SessionScriptManager : ISessionScriptManager, ITransientDependency
    {
        /// <summary>
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SessionScriptManager()
        {
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 获取Session客户端脚本
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();

            script.AppendLine("    abp.session = abp.session || {};");
            script.AppendLine("    abp.session.userId = " + (AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "null") + ";");
            script.AppendLine("    abp.session.tenantId = " + (AbpSession.TenantId.HasValue ? AbpSession.TenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    abp.session.impersonatorUserId = " + (AbpSession.ImpersonatorUserId.HasValue ? AbpSession.ImpersonatorUserId.Value.ToString() : "null") + ";");
            script.AppendLine("    abp.session.impersonatorTenantId = " + (AbpSession.ImpersonatorTenantId.HasValue ? AbpSession.ImpersonatorTenantId.Value.ToString() : "null") + ";");
            script.AppendLine("    abp.session.multiTenancySide = " + ((int)AbpSession.MultiTenancySide) + ";");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}