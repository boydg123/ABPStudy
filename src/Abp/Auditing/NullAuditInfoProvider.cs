using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Auditing
{
    /// <summary>
    /// Default implementation of <see cref="IAuditInfoProvider" />.
    /// <see cref="IAuditInfoProvider"/>的Null对象模式实现
    /// </summary>
    public class DefaultAuditInfoProvider : IAuditInfoProvider, ITransientDependency
    {
        /// <summary>
        /// 客户端信息提供者
        /// </summary>
        public IClientInfoProvider ClientInfoProvider { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultAuditInfoProvider()
        {
            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        /// <summary>
        /// 为审计信息属性赋值
        /// </summary>
        /// <param name="auditInfo">部分属性被赋值的审计信息对象</param>
        public void Fill(AuditInfo auditInfo)
        {
            if (auditInfo.ClientIpAddress.IsNullOrEmpty())
            {
                auditInfo.ClientIpAddress = ClientInfoProvider.ClientIpAddress;
            }

            if (auditInfo.BrowserInfo.IsNullOrEmpty())
            {
                auditInfo.BrowserInfo = ClientInfoProvider.BrowserInfo;
            }

            if (auditInfo.ClientName.IsNullOrEmpty())
            {
                auditInfo.ClientName = ClientInfoProvider.ComputerName;
            }
        }
    }
}