using Abp.Domain.Uow;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Extends <see cref="IConnectionStringResolver"/> to get connection string for given tenant.
    /// <see cref="IConnectionStringResolver"/>的扩展，用于为指定商户获取连接字符串
    /// </summary>
    public interface IDbPerTenantConnectionStringResolver : IConnectionStringResolver
    {
        /// <summary>
        /// Gets the connection string for given args.
        /// 为指定参数获取连接字符串
        /// </summary>
        string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args);
    }
}
