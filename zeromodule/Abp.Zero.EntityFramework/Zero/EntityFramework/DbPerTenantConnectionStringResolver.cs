using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Implements <see cref="IDbPerTenantConnectionStringResolver"/> to dynamically resolve connection string for a multi tenant application.
    /// <see cref="IDbPerTenantConnectionStringResolver"/>的实现，用于动态解析多租户应用程序的连接字符串
    /// </summary>
    public class DbPerTenantConnectionStringResolver : DefaultConnectionStringResolver, IDbPerTenantConnectionStringResolver
    {
        /// <summary>
        /// Reference to the session.
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 当前工作单元提供器
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        /// <summary>
        /// 商户缓存引用
        /// </summary>
        private readonly ITenantCache _tenantCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbPerTenantConnectionStringResolver(
            IAbpStartupConfiguration configuration,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            ITenantCache tenantCache)
            : base(
                  configuration)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _tenantCache = tenantCache;

            AbpSession = NullAbpSession.Instance;
        }
        /// <summary>
        /// 获取Name或连接字符串
        /// </summary>
        /// <param name="args">连接字符串解析参数</param>
        /// <returns></returns>
        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args.MultiTenancySide == MultiTenancySides.Host)
            {
                return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(null, args));
            }

            return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(GetCurrentTenantId(), args));
        }
        /// <summary>
        /// 获取Name或连接字符串
        /// </summary>
        /// <param name="args">每个商户的数据库连接字符串解析参数</param>
        /// <returns></returns>
        public virtual string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args)
        {
            if (args.TenantId == null)
            {
                //Requested for host
                return base.GetNameOrConnectionString(args);
            }

            var tenantCacheItem = _tenantCache.Get(args.TenantId.Value);
            if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
            {
                //Tenant has not dedicated database
                return base.GetNameOrConnectionString(args);
            }

            return tenantCacheItem.ConnectionString;
        }
        /// <summary>
        /// 获取当前的商户ID
        /// </summary>
        /// <returns></returns>
        protected virtual int? GetCurrentTenantId()
        {
            return _currentUnitOfWorkProvider.Current != null
                ? _currentUnitOfWorkProvider.Current.GetTenantId()
                : AbpSession.TenantId;
        }
    }
}
