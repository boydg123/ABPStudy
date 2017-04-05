using Abp.Domain.Entities;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Standard filters of ABP.
    /// ABP的标准过滤器
    /// </summary>
    public static class AbpDataFilters
    {
        /// <summary>
        /// "SoftDelete".Soft delete filter.
        /// 软删除过滤器
        /// Prevents getting deleted data from database.See <see cref="ISoftDelete"/> interface.
        /// 阻止从数据库获取被删除的数据。查看 <see cref="ISoftDelete"/> 接口.
        /// </summary>
        public const string SoftDelete = "SoftDelete";

        /// <summary>
        /// "MustHaveTenant".Tenant filter to prevent getting data that is not belong to current tenant.
        /// 租户过滤器，防止获取不属于当前租户的数据
        /// </summary>
        public const string MustHaveTenant = "MustHaveTenant";

        /// <summary>
        /// "MayHaveTenant".Tenant filter to prevent getting data that is not belong to current tenant.
        /// 租户过滤器，防止获取不属于当前租户的数据
        /// </summary>
        public const string MayHaveTenant = "MayHaveTenant";

        /// <summary>
        /// Standard parameters of ABP.
        /// ABP的标准参数
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// "tenantId". / 租户ID
            /// </summary>
            public const string TenantId = "tenantId";
        }
    }
}