using System;
using System.Linq;
using System.Linq.Expressions;
using Abp.Dependency;
using Abp.EntityFramework.GraphDiff.Configuration;
using RefactorThis.GraphDiff;

namespace Abp.EntityFramework.GraphDiff.Mapping
{
    /// <summary>
    /// Used for resolving mappings for a GraphDiff repository extension methods
    /// 用来为GraphDiff仓储的扩展方法解析映射
    /// </summary>
    public class EntityMappingManager : IEntityMappingManager, ITransientDependency
    {
        /// <summary>
        /// Abp EF GraphDiff模块配置
        /// </summary>
        private readonly IAbpEntityFrameworkGraphDiffModuleConfiguration _moduleConfiguration;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityMappingManager(IAbpEntityFrameworkGraphDiffModuleConfiguration moduleConfiguration)
        {
            _moduleConfiguration = moduleConfiguration;
        }

        /// <summary>
        /// Gets an entity mapping or null for a specified entity type
        /// 为指定的实体类型获取一个实体映射，没获取到则返回Null
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <returns>Entity mapping or null if mapping doesn't exist / 实体映射或null(如果没有找到)</returns>
        public Expression<Func<IUpdateConfiguration<TEntity>, object>> GetEntityMappingOrNull<TEntity>()
        {
            var entityMapping = _moduleConfiguration.EntityMappings.FirstOrDefault(m => m.EntityType == typeof(TEntity));
            var mappingExptession = entityMapping?.MappingExpression as Expression<Func<IUpdateConfiguration<TEntity>, object>>;
            return mappingExptession;
        }
    }
}