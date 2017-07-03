using Abp.Application.Features;
using Abp.Domain.Entities;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Feature setting for a Tenant (<see cref="AbpTenant{TUser}"/>).
    /// 商户(<see cref="AbpTenant{TUser}"/>)的功能设置
    /// </summary>
    public class TenantFeatureSetting : FeatureSetting, IMustHaveTenant
    {
        /// <summary>
        /// 商户的ID
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TenantFeatureSetting()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">The tenant identifier. / 商户ID</param>
        /// <param name="name">Feature name. / 功能名称</param>
        /// <param name="value">Feature value. / 功能值</param>
        public TenantFeatureSetting(int tenantId, string name, string value)
            :base(name, value)
        {
            TenantId = tenantId;
        }
    }
}