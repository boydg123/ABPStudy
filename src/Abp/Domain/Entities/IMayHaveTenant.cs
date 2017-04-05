namespace Abp.Domain.Entities
{
    /// <summary>
    /// Implement this interface for an entity which may optionally have TenantId.
    /// 实现此接口的实体可以包含租户ID(选填)
    /// </summary>
    public interface IMayHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// 实体租户ID
        /// </summary>
        int? TenantId { get; set; }
    }
}