using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;

namespace Derrick.EntityFramework
{
    /// <summary>
    /// ABP Zero数据迁移
    /// </summary>
    public class AbpZeroDbMigrator : AbpZeroDbMigrator<AbpZeroTemplateDbContext, Migrations.Configuration>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元管理</param>
        /// <param name="connectionStringResolver">连接字符串解析器</param>
        /// <param name="iocResolver">IOC解析器</param>
        public AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IIocResolver iocResolver) :
            base(
                unitOfWorkManager,
                connectionStringResolver,
                iocResolver)
        {

        }
    }
}
