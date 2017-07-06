using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Transactions;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// ABP Zero数据迁移
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
    /// <typeparam name="TConfiguration">数据迁移配置类型</typeparam>
    public abstract class AbpZeroDbMigrator<TDbContext, TConfiguration> : IAbpZeroDbMigrator, ITransientDependency
        where TDbContext : DbContext
        where TConfiguration : DbMigrationsConfiguration<TDbContext>, IMultiTenantSeed, new()
    {
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 指定商户的连接字符串解析器
        /// </summary>
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="connectionStringResolver"></param>
        /// <param name="iocResolver"></param>
        protected AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager, 
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IIocResolver iocResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _iocResolver = iocResolver;
        }
        /// <summary>
        /// 为宿主创建或迁移
        /// </summary>
        public virtual void CreateOrMigrateForHost()
        {
            CreateOrMigrate(null);
        }
        /// <summary>
        /// 为商户创建或迁移
        /// </summary>
        /// <param name="tenant">商户</param>
        public virtual void CreateOrMigrateForTenant(AbpTenantBase tenant)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(tenant);
        }
        /// <summary>
        /// 创建或迁移
        /// </summary>
        /// <param name="tenant">商户</param>
        protected virtual void CreateOrMigrate(AbpTenantBase tenant)
        {
            var args = new DbPerTenantConnectionStringResolveArgs(
                tenant == null ? (int?) null : (int?) tenant.Id,
                tenant == null ? MultiTenancySides.Host : MultiTenancySides.Tenant
                );

            args["DbContextType"] = typeof (TDbContext);
            args["DbContextConcreteType"] = typeof(TDbContext);

            var nameOrConnectionString = ConnectionStringHelper.GetConnectionString(
                _connectionStringResolver.GetNameOrConnectionString(args)
            );

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = _iocResolver.ResolveAsDisposable<TDbContext>(new {nameOrConnectionString = nameOrConnectionString}))
                {
                    var dbInitializer = new MigrateDatabaseToLatestVersion<TDbContext, TConfiguration>(
                        true,
                        new TConfiguration
                        {
                            Tenant = tenant
                        });

                    dbInitializer.InitializeDatabase(dbContext.Object);

                    _unitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
