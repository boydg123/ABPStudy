using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Abp.Auditing
{
    /// <summary>
    /// 审计辅助接口
    /// </summary>
    public interface IAuditingHelper
    {
        /// <summary>
        /// 是否需要保存审计
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        /// <summary>
        /// 创建审计信息
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        AuditInfo CreateAuditInfo(MethodInfo method, object[] arguments);

        /// <summary>
        /// 创建审计信息
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        AuditInfo CreateAuditInfo(MethodInfo method, IDictionary<string, object> arguments);

        /// <summary>
        /// 保存审计信息
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        void Save(AuditInfo auditInfo);

        /// <summary>
        /// 异步保存审计信息
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        /// <returns></returns>
        Task SaveAsync(AuditInfo auditInfo);
    }
}