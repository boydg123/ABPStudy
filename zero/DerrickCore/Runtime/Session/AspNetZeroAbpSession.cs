using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using Derrick.MultiTenancy;

namespace Derrick.Runtime.Session
{
    /// <summary>
    /// Extends <see cref="ClaimsAbpSession"/>  (from Abp.Zero library).
    /// <see cref="ClaimsAbpSession"/>的扩展。(从ABP库)
    /// </summary>
    public class AspNetZeroAbpSession : ClaimsAbpSession
    {
        /// <summary>
        /// 商户ID访问器
        /// </summary>
        public ITenantIdAccessor TenantIdAccessor { get; set; }

        /// <summary>
        /// 商户ID
        /// </summary>
        public override int? TenantId
        {
            get
            {
                if (base.TenantId != null)
                {
                    return base.TenantId;
                }

                //Try to find tenant from ITenantIdAccessor, if provided
                return TenantIdAccessor?.GetCurrentTenantIdOrNull(false); //set false to prevent circular usage.
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancy">多商户配置</param>
        public AspNetZeroAbpSession(IMultiTenancyConfig multiTenancy) 
            : base(multiTenancy)
        {

        }
    }
}