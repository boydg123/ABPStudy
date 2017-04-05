using System.Threading.Tasks;

namespace Abp.Auditing
{
    /// <summary>
    /// This interface should be implemented by vendors to make auditing working.Default implementation is <see cref="SimpleLogAuditingStore"/>.
    /// 负责审计工作的服务应该实现此接口<see cref="SimpleLogAuditingStore"/>为默认实现
    /// </summary>
    public interface IAuditingStore
    {
        /// <summary>
        /// Should save audits to a persistent store.
        /// 保存审计信息
        /// </summary>
        /// <param name="auditInfo">Audit informations / 审计信息</param>
        Task SaveAsync(AuditInfo auditInfo);
    }
}