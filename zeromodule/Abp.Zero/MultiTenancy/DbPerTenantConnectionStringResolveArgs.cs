using Abp.Domain.Uow;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// 每个商户的数据库连接字符串参数解析
    /// </summary>
    public class DbPerTenantConnectionStringResolveArgs : ConnectionStringResolveArgs
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="multiTenancySide">多商户双方中的一方</param>
        public DbPerTenantConnectionStringResolveArgs(int? tenantId, MultiTenancySides? multiTenancySide = null)
            : base(multiTenancySide)
        {
            TenantId = tenantId;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="baseArgs">连接字符串解析参数</param>
        public DbPerTenantConnectionStringResolveArgs(int? tenantId, ConnectionStringResolveArgs baseArgs)
        {
            TenantId = tenantId;
            MultiTenancySide = baseArgs.MultiTenancySide;

            foreach (var kvPair in baseArgs)
            {
                Add(kvPair.Key, kvPair.Value);
            }
        }
    }
}