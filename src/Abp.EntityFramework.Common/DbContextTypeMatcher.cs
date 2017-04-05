using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;

namespace Abp.EntityFramework
{
    /// <summary>
    /// <see cref="IDbContextTypeMatcher"/>的默认实现
    /// </summary>
    /// <typeparam name="TBaseDbContext">数据库上下文对象</typeparam>
    public abstract class DbContextTypeMatcher<TBaseDbContext> : IDbContextTypeMatcher, ISingletonDependency
    {
        /// <summary>
        /// 当前的工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <summary>
        /// 数据库上下文类型字典
        /// </summary>
        private readonly Dictionary<Type, List<Type>> _dbContextTypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUnitOfWorkProvider">当前的工作单元提供者</param>
        protected DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _dbContextTypes = new Dictionary<Type, List<Type>>();
        }

        /// <summary>
        /// 填充数据库上下文类型
        /// </summary>
        /// <param name="dbContextTypes">数据库上下文类型</param>
        public void Populate(Type[] dbContextTypes)
        {
            foreach (var dbContextType in dbContextTypes)
            {
                var types = new List<Type>();

                AddWithBaseTypes(dbContextType, types);

                foreach (var type in types)
                {
                    Add(type, dbContextType);
                }
            }
        }

        //TODO: GetConcreteType method can be optimized by extracting/caching MultiTenancySideAttribute attributes for DbContexes.
        /// <summary>
        /// 获取具体的类型
        /// </summary>
        /// <param name="sourceDbContextType">源数据库上下文类型</param>
        /// <returns>具体类型</returns>
        public virtual Type GetConcreteType(Type sourceDbContextType)
        {
            //TODO: This can also get MultiTenancySide to filter dbcontexes

            if (!sourceDbContextType.IsAbstract)
            {
                return sourceDbContextType;
            }
            
            //Get possible concrete types for given DbContext type
            var allTargetTypes = _dbContextTypes.GetOrDefault(sourceDbContextType);

            if (allTargetTypes.IsNullOrEmpty())
            {
                throw new AbpException("Could not find a concrete implementation of given DbContext type: " + sourceDbContextType.AssemblyQualifiedName);
            }

            if (allTargetTypes.Count == 1)
            {
                //Only one type does exists, return it
                return allTargetTypes[0];
            }

            CheckCurrentUow();

            var currentTenancySide = GetCurrentTenancySide();

            var multiTenancySideContexes = GetMultiTenancySideContextTypes(allTargetTypes, currentTenancySide);

            if (multiTenancySideContexes.Count == 1)
            {
                return multiTenancySideContexes[0];
            }

            if (multiTenancySideContexes.Count > 1)
            {
                return GetDefaultDbContextType(multiTenancySideContexes, sourceDbContextType, currentTenancySide);
            }

            return GetDefaultDbContextType(allTargetTypes, sourceDbContextType, currentTenancySide);
        }

        /// <summary>
        /// 检查当前工作单元
        /// </summary>
        private void CheckCurrentUow()
        {
            if (_currentUnitOfWorkProvider.Current == null)
            {
                throw new AbpException("GetConcreteType method should be called in a UOW.");
            }
        }

        /// <summary>
        /// 获取当前租户类型
        /// </summary>
        /// <returns>多租户双方中的一方</returns>
        private MultiTenancySides GetCurrentTenancySide()
        {
            return _currentUnitOfWorkProvider.Current.GetTenantId() == null
                       ? MultiTenancySides.Host
                       : MultiTenancySides.Tenant;
        }

        /// <summary>
        /// 获取多租户方上下文类型
        /// </summary>
        /// <param name="dbContextTypes">数据库上下文类型</param>
        /// <param name="tenancySide">多租户双方中的一方</param>
        /// <returns>类型列表</returns>
        private static List<Type> GetMultiTenancySideContextTypes(List<Type> dbContextTypes, MultiTenancySides tenancySide)
        {
            return dbContextTypes.Where(type =>
            {
                var attrs = type.GetCustomAttributes(typeof(MultiTenancySideAttribute), true);
                if (attrs.IsNullOrEmpty())
                {
                    return false;
                }

                return ((MultiTenancySideAttribute)attrs[0]).Side.HasFlag(tenancySide);
            }).ToList();
        }

        /// <summary>
        /// 获取默认的数据库上下文类型
        /// </summary>
        /// <param name="dbContextTypes">数据库上下文列表</param>
        /// <param name="sourceDbContextType">源数据库上下文类型</param>
        /// <param name="tenancySide">多租户双方中的一方</param>
        /// <returns>类型</returns>
        private static Type GetDefaultDbContextType(List<Type> dbContextTypes, Type sourceDbContextType, MultiTenancySides tenancySide)
        {
            var filteredTypes = dbContextTypes
                .Where(type => !type.IsDefined(typeof(AutoRepositoryTypesAttribute), true))
                .ToList();

            if (filteredTypes.Count == 1)
            {
                return filteredTypes[0];
            }

            filteredTypes = filteredTypes
                .Where(type => !type.IsDefined(typeof(DefaultDbContextAttribute), true))
                .ToList();

            if (filteredTypes.Count == 1)
            {
                return filteredTypes[0];
            }

            throw new AbpException(string.Format(
                "Found more than one concrete type for given DbContext Type ({0}) define MultiTenancySideAttribute with {1}. Found types: {2}.",
                sourceDbContextType,
                tenancySide,
                dbContextTypes.Select(c => c.AssemblyQualifiedName).JoinAsString(", ")
                ));
        }

        /// <summary>
        /// 添加基类型
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="types">类型列表</param>
        private static void AddWithBaseTypes(Type dbContextType, List<Type> types)
        {
            types.Add(dbContextType);
            if (dbContextType != typeof(TBaseDbContext))
            {
                AddWithBaseTypes(dbContextType.BaseType, types);
            }
        }

        /// <summary>
        /// 添加数据库上下文类型
        /// </summary>
        /// <param name="sourceDbContextType">添加源类型</param>
        /// <param name="targetDbContextType">添加目标类型</param>
        private void Add(Type sourceDbContextType, Type targetDbContextType)
        {
            if (!_dbContextTypes.ContainsKey(sourceDbContextType))
            {
                _dbContextTypes[sourceDbContextType] = new List<Type>();
            }

            _dbContextTypes[sourceDbContextType].Add(targetDbContextType);
        }
    }
}