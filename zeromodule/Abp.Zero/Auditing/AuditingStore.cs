using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace Abp.Auditing
{
    /// <summary>
    /// <see cref="IAuditingStore"/>的实现来保存审计信息到数据库
    /// </summary>
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        /// <summary>
        /// 审计日志仓储
        /// </summary>
        private readonly IRepository<AuditLog, long> _auditLogRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuditingStore(IRepository<AuditLog, long> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        /// <summary>
        /// 保存审计信息
        /// </summary>
        /// <param name="auditInfo">审计实体</param>
        /// <returns></returns>
        public Task SaveAsync(AuditInfo auditInfo)
        {
            return _auditLogRepository.InsertAsync(AuditLog.CreateFromAuditInfo(auditInfo));
        }
    }
}