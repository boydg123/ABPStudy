namespace Abp.Domain.Entities
{
    /// <summary>
    /// Implement this interface for an entity which must have TenantId.
    /// 实现此接口的实体必须有租户ID。
    /// </summary>
    public interface IMustHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// 实体的租户ID
        /// </summary>
        int TenantId { get; set; }
    }
}