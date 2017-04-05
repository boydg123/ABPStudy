using Abp.Threading;

namespace Abp.Auditing
{
    /// <summary>
    /// Extension methods for <see cref="IAuditingStore"/>.
    /// <see cref="IAuditingStore"/>扩展方法
    /// </summary>
    public static class AuditingStoreExtensions
    {
        /// <summary>
        /// Should save audits to a persistent store.
        /// 保存审计信息到一个持久化存储中
        /// </summary>
        /// <param name="auditingStore">Auditing store / 审计存储</param>
        /// <param name="auditInfo">Audit informations / 审计信息</param>
        public static void Save(this IAuditingStore auditingStore, AuditInfo auditInfo)
        {
            AsyncHelper.RunSync(() => auditingStore.SaveAsync(auditInfo));
        }
    }
}