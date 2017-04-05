namespace Abp.Runtime.Security
{
    /// <summary>
    /// Used to get ABP-specific claim type names.
    /// 用于获取ABP特定声明的类型名称
    /// </summary>
    public static class AbpClaimTypes
    {
        /// <summary>
        /// TenantId.
        /// 租户ID
        /// </summary>
        public const string TenantId = "http://www.aspnetboilerplate.com/identity/claims/tenantId";

        /// <summary>
        /// ImpersonatorUserId.
        /// 模拟用户ID
        /// </summary>
        public const string ImpersonatorUserId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorUserId";
        
        /// <summary>
        /// ImpersonatorTenantId
        /// 模拟租户ID
        /// </summary>
        public const string ImpersonatorTenantId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorTenantId";
    }
}
