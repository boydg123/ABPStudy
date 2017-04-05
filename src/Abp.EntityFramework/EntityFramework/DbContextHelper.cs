using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Abp.Domain.Entities;
using Abp.Reflection;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 数据库上下文帮助类
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        /// 表名缓存字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, IReadOnlyList<string>> CachedTableNames = new ConcurrentDictionary<string, IReadOnlyList<string>>();

        //TODO: Get entities in different way.. we may not define DbSet for each entity.
        //TODO: 以不同的方式获取实体，我们未必为每个实体定义了DbSet

        /// <summary>
        /// 获取实体类型信息列表
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <returns>实体类型信息列表</returns>
        public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    (ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(IDbSet<>)) ||
                     ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>))) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="type">类型</param>
        /// <returns>表名</returns>
        public static IReadOnlyList<string> GetTableName(this DbContext context, Type type)
        {
            var cacheKey = context.GetType().AssemblyQualifiedName + type.AssemblyQualifiedName;
            return CachedTableNames.GetOrAdd(cacheKey, k =>
            {
                var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

                // Get the part of the model that contains info about the actual CLR types
                // 获取包含实际CLR类型信息的模型的一部分
                var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

                // Get the entity type from the model that maps to the CLR type
                // 从映射到CLR类型的模型中获取实体类型
                var entityType = metadata
                        .GetItems<EntityType>(DataSpace.OSpace)
                        .Single(e => objectItemCollection.GetClrType(e) == type);

                // Get the entity set that uses this entity type
                // 获取使用此实体类型的实体集
                var entitySet = metadata
                    .GetItems<EntityContainer>(DataSpace.CSpace)
                    .Single()
                    .EntitySets
                    .Single(s => s.ElementType.Name == entityType.Name);

                // Find the mapping between conceptual and storage model for this entity set
                // 查找此实体集的概念模型和存储模型之间的映射
                var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                        .Single()
                        .EntitySetMappings
                        .Single(s => s.EntitySet == entitySet);

                // Find the storage entity sets (tables) that the entity is mapped
                // 查找实体映射的存储实体集（表）
                var tables = mapping
                    .EntityTypeMappings.Single()
                    .Fragments;

                // Return the table name from the storage entity set
                // 从存储实体集返回表名
                return tables.Select(f => (string)f.StoreEntitySet.MetadataProperties["Table"].Value ?? f.StoreEntitySet.Name).ToImmutableList();
            });
        }
    }
}
