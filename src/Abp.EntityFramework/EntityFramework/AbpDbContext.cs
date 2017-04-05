using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Extensions;
using Abp.Reflection;
using Abp.Runtime.Session;
using Abp.Timing;
using Castle.Core.Logging;
using EntityFramework.DynamicFilters;

namespace Abp.EntityFramework
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// 应用程序所有数据库上下文类的基类
    /// </summary>
    public abstract class AbpDbContext : DbContext, ITransientDependency, IShouldInitialize
    {
        /// <summary>
        /// Used to get current session values.
        /// 用于获取当前的Session值
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Used to trigger entity change events.
        /// 用于触发实体更改事件
        /// </summary>
        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        /// <summary>
        /// Reference to the logger.
        /// 日志记录器的引用
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the event bus.
        /// 事件总线的引用
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        /// Reference to GUID generator.
        /// GUID生成器的引用
        /// </summary>
        public IGuidGenerator GuidGenerator { get; set; }

        /// <summary>
        /// Reference to the current UOW provider.
        /// 当前工作单元提供者的引用
        /// </summary>
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// Reference to multi tenancy configuration.
        /// 多租户配置的引用
        /// </summary>
        public IMultiTenancyConfig MultiTenancyConfig { get; set; }

        /// <summary>
        /// Constructor.Uses <see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// 构造函数。使用<see cref="IAbpStartupConfiguration.DefaultNameOrConnectionString"/>作为一个连接字符串
        /// </summary>
        protected AbpDbContext()
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpDbContext(DbCompiledModel model)
            : base(model)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// 初始化数据库上下文
        /// </summary>
        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
            RegisterToChanges();
        }

        /// <summary>
        /// 注册到Change事件
        /// </summary>
        private void RegisterToChanges()
        {
            ((IObjectContextAdapter)this)
                .ObjectContext
                .ObjectStateManager
                .ObjectStateManagerChanged += ObjectStateManager_ObjectStateManagerChanged;
        }

        /// <summary>
        /// 对象状态更改事件管理
        /// </summary>
        /// <param name="sender">触发者</param>
        /// <param name="e">参数</param>
        protected virtual void ObjectStateManager_ObjectStateManagerChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            var contextAdapter = (IObjectContextAdapter)this;
            if (e.Action != CollectionChangeAction.Add)
            {
                return;
            }

            var entry = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntry(e.Element);
            switch (entry.State)
            {
                case EntityState.Added:
                    CheckAndSetId(entry.Entity);
                    CheckAndSetMustHaveTenantIdProperty(entry.Entity);
                    SetCreationAuditProperties(entry.Entity, GetAuditUserId());
                    break;
                    //case EntityState.Deleted: //It's not going here at all
                    //    SetDeletionAuditProperties(entry.Entity, GetAuditUserId());
                    //    break;
            }
        }

        /// <summary>
        /// 为注入属性设置Null
        /// </summary>
        private void SetNullsForInjectedProperties()
        {
            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            GuidGenerator = SequentialGuidGenerator.Instance;
            EventBus = NullEventBus.Instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            Database.Initialize(false);
            this.SetFilterScopedParameterValue(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId ?? 0);
            this.SetFilterScopedParameterValue(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, AbpSession.TenantId);
        }

        /// <summary>
        /// 在完成对派生上下文的模型的初始化后，并在该模型已锁定并用于初始化上下文之前，将调用此方法。虽然此方法的默认实现不执行任何操作，但可在派生类中重写此方法，这样便能在锁定模型之前对其进行进一步的配置。
        /// </summary>
        /// <param name="modelBuilder">定义要创建的上下文的模型的生成器</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Filter(AbpDataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);
            modelBuilder.Filter(AbpDataFilters.MustHaveTenant, (IMustHaveTenant t, int tenantId) => t.TenantId == tenantId || (int?)t.TenantId == null, 0); //While "(int?)t.TenantId == null" seems wrong, it's needed. See https://github.com/jcachat/EntityFramework.DynamicFilters/issues/62#issuecomment-208198058
            modelBuilder.Filter(AbpDataFilters.MayHaveTenant, (IMayHaveTenant t, int? tenantId) => t.TenantId == tenantId, 0);
        }

        /// <summary>
        /// 提交当前单元操作的更改
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public override int SaveChanges()
        {
            try
            {
                ApplyAbpConcepts();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        /// <summary>
        /// 异步提交当前单元操作的更改
        /// </summary>
        /// <param name="cancellationToken">取消操作的通知</param>
        /// <returns>操作影响的行数</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                ApplyAbpConcepts();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        /// <summary>
        /// 应用Abp的概念
        /// </summary>
        protected virtual void ApplyAbpConcepts()
        {
            var userId = GetAuditUserId();

            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        CheckAndSetId(entry.Entity);
                        CheckAndSetMustHaveTenantIdProperty(entry.Entity);
                        CheckAndSetMayHaveTenantIdProperty(entry.Entity);
                        SetCreationAuditProperties(entry.Entity, userId);
                        EntityChangeEventHelper.TriggerEntityCreatingEvent(entry.Entity);
                        EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry, userId);
                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            SetDeletionAuditProperties(entry.Entity, userId);
                            EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                            EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        }
                        else
                        {
                            EntityChangeEventHelper.TriggerEntityUpdatingEvent(entry.Entity);
                            EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompleted(entry.Entity);
                        }

                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry.Entity, userId);
                        EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                        EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        break;
                }

                TriggerDomainEvents(entry.Entity);
            }
        }

        /// <summary>
        /// 触发领域事件
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        protected virtual void TriggerDomainEvents(object entityAsObj)
        {
            var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
            {
                return;
            }

            var domainEvents = generatesDomainEventsEntity.DomainEvents.ToList();
            generatesDomainEventsEntity.DomainEvents.Clear();

            foreach (var domainEvent in domainEvents)
            {
                EventBus.Trigger(domainEvent.GetType(), entityAsObj, domainEvent);
            }
        }

        /// <summary>
        /// 检查并且设置ID
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        protected virtual void CheckAndSetId(object entityAsObj)
        {
            //Set GUID Ids
            var entity = entityAsObj as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                var entityType = ObjectContext.GetObjectType(entityAsObj.GetType());
                var idProperty = entityType.GetProperty("Id");
                var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    entity.Id = GuidGenerator.Create();
                }
            }
        }

        /// <summary>
        /// 检查并且设置必须包含租户属性的对象
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        {
            //Only set IMustHaveTenant entities
            if (!(entityAsObj is IMustHaveTenant))
            {
                return;
            }

            var entity = entityAsObj.As<IMustHaveTenant>();

            //Don't set if it's already set
            if (entity.TenantId != 0)
            {
                return;
            }

            var currentTenantId = GetCurrentTenantIdOrNull();

            if (currentTenantId != null)
            {
                entity.TenantId = currentTenantId.Value;
            }
            else
            {
                throw new AbpException("Can not set TenantId to 0 for IMustHaveTenant entities!");
            }
        }

        /// <summary>
        /// 检查并且设置可能包含租户属性的对象
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        {
            //Only set IMayHaveTenant entities
            if (!(entityAsObj is IMayHaveTenant))
            {
                return;
            }

            var entity = entityAsObj.As<IMayHaveTenant>();

            //Don't set if it's already set
            if (entity.TenantId != null)
            {
                return;
            }

            //Only works for single tenant applications
            if (MultiTenancyConfig?.IsEnabled ?? false)
            {
                return;
            }

            //Don't set if MayHaveTenant filter is disabled
            if (!this.IsFilterEnabled(AbpDataFilters.MayHaveTenant))
            {
                return;
            }

            entity.TenantId = GetCurrentTenantIdOrNull();
        }

        /// <summary>
        /// 设置创建审计属性
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        /// <param name="userId">用户ID</param>
        protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
        {
            var entityWithCreationTime = entityAsObj as IHasCreationTime;
            if (entityWithCreationTime == null)
            {
                return;
            }

            if (entityWithCreationTime.CreationTime == default(DateTime))
            {
                entityWithCreationTime.CreationTime = Clock.Now;
            }

            if (userId.HasValue && entityAsObj is ICreationAudited)
            {
                var entity = entityAsObj as ICreationAudited;
                if (entity.CreatorUserId == null)
                {
                    if (entity is IMayHaveTenant || entity is IMustHaveTenant)
                    {
                        //Sets CreatorUserId only if current user is in same tenant/host with the given entity
                        if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId) ||
                            (entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId))
                        {
                            entity.CreatorUserId = userId;
                        }
                    }
                    else
                    {
                        entity.CreatorUserId = userId;
                    }
                }
            }
        }

        /// <summary>
        /// 设置修改审计属性
        /// </summary>
        /// <param name="entry">数据实体</param>
        /// <param name="userId">用户ID</param>
        protected virtual void SetModificationAuditProperties(DbEntityEntry entry, long? userId)
        {
            if (entry.Entity is IHasModificationTime)
            {
                entry.Cast<IHasModificationTime>().Entity.LastModificationTime = Clock.Now;
            }

            if (entry.Entity is IModificationAudited)
            {
                var entity = entry.Cast<IModificationAudited>().Entity;

                if (userId == null)
                {
                    entity.LastModifierUserId = null;
                    return;
                }

                //Special check for multi-tenant entities
                if (entity is IMayHaveTenant || entity is IMustHaveTenant)
                {
                    //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
                    if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId) ||
                        (entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId))
                    {
                        entity.LastModifierUserId = userId;
                    }
                    else
                    {
                        entity.LastModifierUserId = null;
                    }
                }
                else
                {
                    entity.LastModifierUserId = userId;
                }
            }
        }

        /// <summary>
        /// 为软删除取消删除
        /// </summary>
        /// <param name="entry">数据实体</param>
        protected virtual void CancelDeletionForSoftDelete(DbEntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            var softDeleteEntry = entry.Cast<ISoftDelete>();

            softDeleteEntry.State = EntityState.Unchanged; //TODO: Or Modified? IsDeleted = true makes it modified?
            softDeleteEntry.Entity.IsDeleted = true;
        }

        /// <summary>
        /// 设置删除审计属性
        /// </summary>
        /// <param name="entityAsObj">实体对象</param>
        /// <param name="userId">用户ID</param>
        protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = entityAsObj.As<IHasDeletionTime>();

                if (entity.DeletionTime == null)
                {
                    entity.DeletionTime = Clock.Now;
                }
            }

            if (entityAsObj is IDeletionAudited)
            {
                var entity = entityAsObj.As<IDeletionAudited>();

                if (entity.DeleterUserId != null)
                {
                    return;
                }

                if (userId == null)
                {
                    entity.DeleterUserId = null;
                    return;
                }

                //Special check for multi-tenant entities
                if (entity is IMayHaveTenant || entity is IMustHaveTenant)
                {
                    //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
                    if ((entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId) ||
                        (entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId))
                    {
                        entity.DeleterUserId = userId;
                    }
                    else
                    {
                        entity.DeleterUserId = null;
                    }
                }
                else
                {
                    entity.DeleterUserId = userId;
                }
            }
        }

        /// <summary>
        /// 记录数据实体验证异常日志
        /// </summary>
        /// <param name="exception">数据实体验证异常对象</param>
        protected virtual void LogDbEntityValidationException(DbEntityValidationException exception)
        {
            Logger.Error("There are some validation errors while saving changes in EntityFramework:");
            foreach (var ve in exception.EntityValidationErrors.SelectMany(eve => eve.ValidationErrors))
            {
                Logger.Error(" - " + ve.PropertyName + ": " + ve.ErrorMessage);
            }
        }

        /// <summary>
        /// 获取审计用户ID
        /// </summary>
        /// <returns></returns>
        protected virtual long? GetAuditUserId()
        {
            if (AbpSession.UserId.HasValue &&
                CurrentUnitOfWorkProvider != null &&
                CurrentUnitOfWorkProvider.Current != null &&
                CurrentUnitOfWorkProvider.Current.GetTenantId() == AbpSession.TenantId)
            {
                return AbpSession.UserId;
            }

            return null;
        }

        /// <summary>
        /// 获取当前租户ID，或者null
        /// </summary>
        /// <returns>租户ID，没找到则返回Null</returns>
        protected virtual int? GetCurrentTenantIdOrNull()
        {
            if (CurrentUnitOfWorkProvider?.Current != null)
            {
                return CurrentUnitOfWorkProvider.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}
