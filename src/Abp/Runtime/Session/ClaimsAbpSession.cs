using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Security;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements <see cref="IAbpSession"/> to get session properties from claims of <see cref="Thread.CurrentPrincipal"/>.
    /// <see cref="IAbpSession"/>的实现，从<see cref="Thread.CurrentPrincipal"/>声明中获取session属性
    /// </summary>
    public class ClaimsAbpSession : IAbpSession, ISingletonDependency
    {
        /// <summary>
        /// 获取当前用户编号，它是可空的，如果用户没有登录
        /// </summary>
        public virtual long? UserId
        {
            get
            {
                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }

                long userId;
                if (!long.TryParse(userIdClaim.Value, out userId))
                {
                    return null;
                }

                return userId;
            }
        }

        /// <summary>
        /// 获取当前租户ID,这个租户ID应该是<see cref="UserId"/>的租户ID,它可为null如果给定的<see cref="UserId"/>是宿主用户，或者用户没有登录
        /// </summary>
        public virtual int? TenantId
        {
            get
            {
                if (!MultiTenancy.IsEnabled)
                {
                    return MultiTenancyConsts.DefaultTenantId;
                }

                var tenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
                if (string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt32(tenantIdClaim.Value);
            }
        }

        /// <summary>
        /// 虚拟的用户ID，如果用户正在执行方法代表(<see cref="UserId"/>)则应该填充它
        /// </summary>
        public virtual long? ImpersonatorUserId
        {
            get
            {
                var impersonatorUserIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorUserId);
                if (string.IsNullOrEmpty(impersonatorUserIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt64(impersonatorUserIdClaim.Value);
            }
        }

        /// <summary>
        /// 虚拟的租户ID,如果一个用户使用<see cref="UserId"/>代表使用 <see cref="ImpersonatorUserId"/>执行方法，则应该填充它
        /// </summary>
        public virtual int? ImpersonatorTenantId
        {
            get
            {
                if (!MultiTenancy.IsEnabled)
                {
                    return MultiTenancyConsts.DefaultTenantId;
                }

                var impersonatorTenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorTenantId);
                if (string.IsNullOrEmpty(impersonatorTenantIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt32(impersonatorTenantIdClaim.Value);
            }
        }

        /// <summary>
        /// 表示多租户双方中的一方
        /// </summary>
        public virtual MultiTenancySides MultiTenancySide
        {
            get
            {
                return MultiTenancy.IsEnabled && !TenantId.HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }
        }

        public IPrincipalAccessor PrincipalAccessor { get; set; } //TODO: Convert to constructor-injection

        /// <summary>
        /// 多租户配置
        /// </summary>
        protected readonly IMultiTenancyConfig MultiTenancy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancy"></param>
        public ClaimsAbpSession(IMultiTenancyConfig multiTenancy)
        {
            MultiTenancy = multiTenancy;
            PrincipalAccessor = DefaultPrincipalAccessor.Instance;
        }
    }
}