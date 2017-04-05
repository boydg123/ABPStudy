using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFramework.Utils;
using Abp.Extensions;
using Abp.MultiTenancy;
using Castle.Core.Internal;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// EF工作单元的实现
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// 激活的数据库上下文
        /// </summary>
        protected IDictionary<string, DbContext> ActiveDbContexts { get; private set; }

        /// <summary>
        /// IOC解析器
        /// </summary>
        protected IIocResolver IocResolver { get; private set; }

        /// <summary>
        /// 事物范围
        /// </summary>
        protected TransactionScope CurrentTransaction;

        /// <summary>
        /// 数据库上下文解析器
        /// </summary>
        private readonly IDbContextResolver _dbContextResolver;

        /// <summary>
        /// 数据库上下文类型匹配器
        /// </summary>
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        /// <summary>
        /// Creates a new <see cref="EfUnitOfWork"/>.
        /// 构造函数
        /// </summary>
        public EfUnitOfWork(
            IIocResolver iocResolver,
            IConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver,
            IEfUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions, 
            IDbContextTypeMatcher dbContextTypeMatcher)
            : base(
                  connectionStringResolver, 
                  defaultOptions,
                  filterExecuter)
        {
            IocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;

            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        /// <summary>
        /// 开始工作单元
        /// </summary>
        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = Options.IsolationLevel.GetValueOrDefault(IsolationLevel.ReadUncommitted),
                };

                if (Options.Timeout.HasValue)
                {
                    transactionOptions.Timeout = Options.Timeout.Value;
                }

                CurrentTransaction = new TransactionScope(
                    Options.Scope.GetValueOrDefault(TransactionScopeOption.Required),
                    transactionOptions,
                    Options.AsyncFlowOption.GetValueOrDefault(TransactionScopeAsyncFlowOption.Enabled)
                    );
            }
        }

        /// <summary>
        /// 现在，保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        public override void SaveChanges()
        {
            ActiveDbContexts.Values.ForEach(SaveChangesInDbContext);
        }

        /// <summary>
        /// 现在，保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }

        /// <summary>
        /// 获取所有的数据库上下文
        /// </summary>
        /// <returns>数据库上下文列表</returns>
        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        /// <summary>
        /// 便完成工作单元
        /// </summary>
        protected override void CompleteUow()
        {
            SaveChanges();
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Complete();
            }

            DisposeUow();
        }

        /// <summary>
        /// 完成工作单元 - 异步
        /// </summary>
        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Complete();
            }

            DisposeUow();
        }

        /// <summary>
        /// 获取或创建数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文对象</typeparam>
        /// <param name="multiTenancySide">多租户双方中的一方</param>
        /// <returns>数据库上下文对象</returns>
        public virtual TDbContext GetOrCreateDbContext<TDbContext>(MultiTenancySides? multiTenancySide = null)
            where TDbContext : DbContext
        {
            var concreteDbContextType = _dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));

            var connectionStringResolveArgs = new ConnectionStringResolveArgs(multiTenancySide);
            connectionStringResolveArgs["DbContextType"] = typeof(TDbContext);
            connectionStringResolveArgs["DbContextConcreteType"] = concreteDbContextType;
            var connectionString = ResolveConnectionString(connectionStringResolveArgs);

            var dbContextKey = concreteDbContextType.FullName + "#" + connectionString;

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {

                dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString);

                ((IObjectContextAdapter)dbContext).ObjectContext.ObjectMaterialized += (sender, args) =>
                {
                    ObjectContext_ObjectMaterialized(dbContext, args);
                };

                FilterExecuter.As<IEfUnitOfWorkFilterExecuter>().ApplyCurrentFilters(this, dbContext);

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }

        /// <summary>
        /// 销毁工作单元
        /// </summary>
        protected override void DisposeUow()
        {
            ActiveDbContexts.Values.ForEach(Release);
            ActiveDbContexts.Clear();

            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }

        /// <summary>
        /// 在数据库山下文中保存所有的更改
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 在数据库山下文中保存所有的更改 - 异步
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <returns></returns>
        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 释放数据库上下文
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
            IocResolver.Release(dbContext);
        }

        /// <summary>
        /// 对象上下文 - 实对象
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="e">参数</param>
        private static void ObjectContext_ObjectMaterialized(DbContext dbContext, ObjectMaterializedEventArgs e)
        {
            var entityType = ObjectContext.GetObjectType(e.Entity.GetType());

            dbContext.Configuration.AutoDetectChangesEnabled = false;
            var previousState = dbContext.Entry(e.Entity).State;

            DateTimePropertyInfoHelper.NormalizeDatePropertyKinds(e.Entity, entityType);

            dbContext.Entry(e.Entity).State = previousState;
            dbContext.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}