using System.Threading.Tasks;

namespace Abp.Organizations
{
    /// <summary>
    /// Used to get settings related to OrganizationUnits.
    /// 用于获取组织架构相关的设置
    /// </summary>
    public interface IOrganizationUnitSettings
    {
        /// <summary>
        /// Get Maximum allowed organization unit membership count for a user.Returns value for current tenant.
        /// 获取用户允许的最大组织架构成员数量。返回当前商户的值
        /// </summary>
        int MaxUserMembershipCount { get; }

        /// <summary>
        /// Gets Maximum allowed organization unit membership count for a user.Returns value for given tenant.
        /// 获取用户允许的最大组织架构成员数量。返回当前商户的值
        /// </summary>
        /// <param name="tenantId">The tenant Id or null for the host.</param>
        Task<int> GetMaxUserMembershipCountAsync(int? tenantId);

        /// <summary>
        /// Sets Maximum allowed organization unit membership count for a user.
        /// 设置用户允许的最大组织架构成员数量
        /// </summary>
        /// <param name="tenantId">The tenant Id or null for the host. / 商户ID或Null(商户是宿主商户)</param>
        /// <param name="value">Setting value. / 设置的值</param>
        /// <returns></returns>
        Task SetMaxUserMembershipCountAsync(int? tenantId, int value);
    }
}