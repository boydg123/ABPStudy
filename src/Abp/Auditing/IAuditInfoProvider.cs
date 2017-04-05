namespace Abp.Auditing
{
    /// <summary>
    /// Provides an interface to provide audit informations in the upper layers.
    /// 为上层提供审计信息的接口
    /// </summary>
    public interface IAuditInfoProvider
    {
        /// <summary>
        /// Called to fill needed properties.
        /// 为审计信息属性赋值
        /// </summary>
        /// <param name="auditInfo">Audit info that is partially filled / 部分属性被赋值的审计信息对象</param>
        void Fill(AuditInfo auditInfo);
    }
}