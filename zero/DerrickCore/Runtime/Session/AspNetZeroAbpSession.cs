using Abp.Configuration.Startup;
using Abp.Runtime.Session;
using Derrick.MultiTenancy;

namespace Derrick.Runtime.Session
{
    /// <summary>
    /// Extends <see cref="ClaimsAbpSession"/>  (from Abp.Zero library).
    /// </summary>
    public class AspNetZeroAbpSession : ClaimsAbpSession
    {
        public ITenantIdAccessor TenantIdAccessor { get; set; }

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

        public AspNetZeroAbpSession(IMultiTenancyConfig multiTenancy) 
            : base(multiTenancy)
        {

        }
    }
}