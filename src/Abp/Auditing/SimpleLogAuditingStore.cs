using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abp.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore"/> to simply write audits to logs.
    /// 实现接口<see cref="IAuditingStore"/>，简单记录审计日志.
    /// </summary>
    public class SimpleLogAuditingStore : IAuditingStore
    {
        /// <summary>
        /// Singleton instance.
        /// 单例
        /// </summary>
        public static SimpleLogAuditingStore Instance { get; } = new SimpleLogAuditingStore();

        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleLogAuditingStore()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        /// <returns></returns>
        public Task SaveAsync(AuditInfo auditInfo)
        {
            if (auditInfo.Exception == null)
            {
                Logger.Info(auditInfo.ToString());
            }
            else
            {
                Logger.Warn(auditInfo.ToString());
            }

            return Task.FromResult(0);
        }
    }
}