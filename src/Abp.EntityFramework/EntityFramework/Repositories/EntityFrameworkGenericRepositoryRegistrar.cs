using System;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Reflection.Extensions;
using Castle.Core.Logging;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// <see cref="IEntityFrameworkGenericRepositoryRegistrar"/>实现。EF通用仓储注册器
    /// </summary>
    public class EntityFrameworkGenericRepositoryRegistrar : IEntityFrameworkGenericRepositoryRegistrar, ITransientDependency
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityFrameworkGenericRepositoryRegistrar()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 注册数据库上下文
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="iocManager">IOC管理器</param>
        public void RegisterForDbContext(Type dbContextType, IIocManager iocManager)
        {
            var autoRepositoryAttr = dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ??
                                     EfAutoRepositoryTypes.Default;

            foreach (var entityTypeInfo in DbContextHelper.GetEntityTypeInfos(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityTypeInfo.EntityType);
                    if (!iocManager.IsRegistered(genericRepositoryType))
                    {
                        var implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                                ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                                : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType);

                        iocManager.Register(
                            genericRepositoryType,
                            implType,
                            DependencyLifeStyle.Transient
                            );
                    }
                }

                var genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);
                if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                {
                    var implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                                ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                                : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    iocManager.Register(
                        genericRepositoryTypeWithPrimaryKey,
                        implType,
                        DependencyLifeStyle.Transient
                        );
                }
            }
        }
    }
}