using System;
using System.Linq.Expressions;
using Abp.Domain.Entities;
using RefactorThis.GraphDiff;

namespace Abp.EntityFramework.GraphDiff.Mapping
{
    /// <summary>
    /// Helper class for creating entity 
    /// 创建实体的帮助类
    /// </summary>
    public static class MappingExpressionBuilder
    {
        /// <summary>
        /// A shortcut of <see cref="For{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
        /// 使用(<see cref="int"/>)为主键类型的<see cref="For{TEntity,TPrimaryKey}"/>的快捷方式
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <param name="expression">实体映射条件表达式</param>
        /// <returns>实体映射</returns>
        public static EntityMapping For<TEntity>(Expression<Func<IUpdateConfiguration<TEntity>, object>> expression)
            where TEntity : class, IEntity
        {
            return For<TEntity, int>(expression);
        }

        /// <summary>
        /// Build a mapping for an entity with a specified primary key
        /// 为具有指定主键的实体生成映射
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">Primary key type of the entity / 实体的主键类型</typeparam>
        /// <param name="expression">实体映射条件表达式</param>
        /// <returns>实体映射</returns>
        public static EntityMapping For<TEntity, TPrimaryKey>(Expression<Func<IUpdateConfiguration<TEntity>, object>> expression)
            where TPrimaryKey : IEquatable<TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return new EntityMapping(typeof(TEntity), expression);
        }
    }
}
