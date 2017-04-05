namespace Abp
{
    /// <summary>
    /// Interface to get a user identifier.
    /// 获取用户标识的接口
    /// </summary>
    public interface IUserIdentifier
    {
        /// <summary>
        /// Tenant Id. Can be null for host users.
        /// 租户ID，宿主User可以为null
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Id of the user.
        /// 用户的ID
        /// </summary>
        long UserId { get; }
    }
}