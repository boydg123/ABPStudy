using System;
using System.Threading;
using System.Web;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Derrick.MultiTenancy;


namespace Derrick.Web.MultiTenancy
{
    /// <summary>
    /// Implements <see cref="ITenantIdAccessor"/> to try to get current tenant id
    /// using <see cref="ITenancyNameFinder"/> and using <see cref="HttpContext"/>
    /// </summary>
    public class TenantIdAccessor : ITenantIdAccessor, ISingletonDependency
    {
        private const string HttpContextKey = "AbpZeroCurrentTenantCacheItem";

        private readonly ITenantCache _tenantCache;
        private readonly IIocResolver _iocResolver;
        
        private readonly AsyncLocal<int?> _currentTenant;
        private readonly Lazy<IAbpSession> _abpSession;
        private readonly Lazy<ITenancyNameFinder> _tenancyNameFinder;

        private int? CurrentTenantId
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    _currentTenant.Value = HttpContext.Current.Items[HttpContextKey] as int?;
                    return _currentTenant.Value;
                }

                return _currentTenant.Value;
            }

            set
            {
                _currentTenant.Value = value;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[HttpContextKey] = _currentTenant.Value;
                }
            }
        }

        public TenantIdAccessor(
            ITenantCache tenantCache,
            IIocResolver iocResolver)
        {
            _tenantCache = tenantCache;
            _iocResolver = iocResolver;

            _currentTenant = new AsyncLocal<int?>();
            _abpSession = new Lazy<IAbpSession>(() => _iocResolver.Resolve<IAbpSession>(), true);
            _tenancyNameFinder = new Lazy<ITenancyNameFinder>(() => _iocResolver.Resolve<ITenancyNameFinder>(), true);
        }

        /// <summary>
        /// Gets current tenant id.
        /// Use <see cref="IAbpSession.TenantId"/> wherever possible (if user logged in).
        /// This method tries to get current tenant id even if current user did not log in.
        /// </summary>
        /// <param name="useSession">Set false to skip session usage</param>
        public int? GetCurrentTenantIdOrNull(bool useSession = true)
        {
            if (useSession)
            {
                return _abpSession.Value.TenantId;
            }

            return CurrentTenantId;
        }

        /// <summary>
        /// This method is called on request beginning to obtain current tenant id.
        /// </summary>
        public void SetCurrentTenantId()
        {
            var tenancyName = _tenancyNameFinder.Value.GetCurrentTenancyNameOrNull();
            if (tenancyName == null)
            {
                CurrentTenantId = null;
                return;
            }

            CurrentTenantId = _tenantCache.GetOrNull(tenancyName)?.Id;
        }
    }
}