using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.TestBase.Runtime.Session
{
    /// <summary>
    /// 测试 ABP Session
    /// </summary>
    public class TestAbpSession : IAbpSession, ISingletonDependency
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int? TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    return 1;
                }
                
                return _tenantId;
            }
            set
            {
                if (!_multiTenancy.IsEnabled && value != 1 && value != null)
                {
                    throw new AbpException("Can not set TenantId since multi-tenancy is not enabled. Use IMultiTenancyConfig.IsEnabled to enable it.");
                }

                _tenantId = value;
            }
        }

        /// <summary>
        /// 多租户双方中的一方
        /// </summary>
        public MultiTenancySides MultiTenancySide { get { return GetCurrentMultiTenancySide(); } }

        /// <summary>
        /// 虚拟的用户ID，如果用户正在执行方法代表(<see cref="UserId"/>)则应该填充它
        /// </summary>
        public long? ImpersonatorUserId { get; set; }

        /// <summary>
        /// 虚拟的租户ID,如果一个用户使用<see cref="UserId"/>代表使用 <see cref="ImpersonatorUserId"/>执行方法，则应该填充它
        /// </summary>
        public int? ImpersonatorTenantId { get; set; }

        /// <summary>
        /// 多租户
        /// </summary>
        private readonly IMultiTenancyConfig _multiTenancy;

        /// <summary>
        /// 租户ID
        /// </summary>
        private int? _tenantId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancy"></param>
        public TestAbpSession(IMultiTenancyConfig multiTenancy)
        {
            _multiTenancy = multiTenancy;
        }

        /// <summary>
        /// 获取当前租户
        /// </summary>
        /// <returns>双方中的一方</returns>
        private MultiTenancySides GetCurrentMultiTenancySide()
        {
            return _multiTenancy.IsEnabled && !TenantId.HasValue
                ? MultiTenancySides.Host
                : MultiTenancySides.Tenant;
        }
    }
}