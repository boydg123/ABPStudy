using System;
using System.Linq.Expressions;
using RefactorThis.GraphDiff;

namespace Abp.EntityFramework.GraphDiff.Mapping
{
    /// <summary>
    /// 实体映射管理器接口
    /// </summary>
    public interface IEntityMappingManager
    {
        /// <summary>
        /// 获取实体映射的条件
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <returns>实体映射条件表达式</returns>
        Expression<Func<IUpdateConfiguration<TEntity>, object>> GetEntityMappingOrNull<TEntity>();
    }
}