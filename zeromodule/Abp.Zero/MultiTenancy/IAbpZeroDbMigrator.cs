namespace Abp.MultiTenancy
{
    /// <summary>
    /// ABP Zero数据迁移
    /// </summary>
    public interface IAbpZeroDbMigrator
    {
        /// <summary>
        /// 为宿主创建或迁移
        /// </summary>
        void CreateOrMigrateForHost();
        /// <summary>
        /// 为商户创建或迁移
        /// </summary>
        /// <param name="tenant"></param>
        void CreateOrMigrateForTenant(AbpTenantBase tenant);
    }
}
