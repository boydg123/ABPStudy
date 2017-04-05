using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework.GraphDiff.Mapping;
using Abp.EntityFramework.Repositories;
using RefactorThis.GraphDiff;

namespace Abp.EntityFramework.GraphDiff.Extensions
{
    /// <summary>
    /// This class is an extension for GraphDiff library which provides a possibility to attach a detached graphs (i.e. entities) to a context.
    /// 此类是GraphDiff库的一个扩展，能提供为上下文附加一个分离的graphs(即实体)的可能性。
    /// Attaching a whole graph using this methods updates all entity's navigation properties on entity creation or modification.
    /// 使用此方法附加graph，在实体创建或修改时更新所有实体的导航属性
    /// </summary>
    public static class GraphDiffExtensions
    {
        /// <summary>
        /// Attaches an <paramref name="entity"/> (as a detached graph) to a context.
        /// 为上下文附加一个<paramref name="entity"/>(作为一个分离的graph)
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">Primary key type of the entity / 拥有主键的实体类型</typeparam>
        /// <param name="repository">仓储接口</param>
        /// <param name="entity">实体</param>
        /// <returns>Attached entity / 附加的实体</returns>
        public static TEntity AttachGraph<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>, new()
        {
            var iocResolver = ((AbpRepositoryBase<TEntity, TPrimaryKey>)repository).IocResolver;

            using (var mappingManager = iocResolver.ResolveAsDisposable<IEntityMappingManager>())
            {
                var mapping = mappingManager.Object.GetEntityMappingOrNull<TEntity>();
                return repository
                    .GetDbContext()
                    .UpdateGraph(entity, mapping);
            }
        }

        /// <summary>
        /// Attaches an <paramref name="entity"/> (as a detached graph) to a context.
        /// 为上下文附加一个<paramref name="entity"/>(作为一个分离的graph)
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">Primary key type of the entity / 拥有主键的实体类型</typeparam>
        /// <param name="repository">仓储接口</param>
        /// <param name="entity">实体</param>
        /// <returns>Attached entity / 附加的实体</returns>
        public static Task<TEntity> AttachGraphAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>, new()
        {
            return Task.FromResult(AttachGraph(repository, entity));
        }

        /// <summary>
        /// Attaches an <paramref name="entity"/> (as a detached graph) to a context and gets it's Id.
        /// 为上下文附加一个<paramref name="entity"/>(作为一个分离的graph),并且获取它的Id
        /// It may require to save current unit of work to be able to retrieve id.
        /// 它可能需要保存当前的工作单元，以便能恢复ID
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">Primary key type of the entity / 拥有主键的实体类型</typeparam>
        /// <param name="repository">仓储接口</param>
        /// <param name="entity">实体</param>
        /// <returns>Id of the entity / 附加的实体</returns>
        public static TPrimaryKey AttachGraphAndGetId<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>, new()
        {
            return AttachGraph(repository, entity).Id;
        }

        /// <summary>
        /// Attaches an <paramref name="entity"/> (as a detached graph) to a context and gets it's Id.
        /// 为上下文附加一个<paramref name="entity"/>(作为一个分离的graph),并且获取它的Id
        /// It may require to save current unit of work to be able to retrieve id.
        /// 它可能需要保存当前的工作单元，以便能恢复ID
        /// </summary>
        /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">Primary key type of the entity /拥有主键的实体类型</typeparam>
        /// <param name="repository">仓储接口</param>
        /// <param name="entity">实体</param>
        /// <returns>Id of the entity / 附加的实体</returns>
        public static Task<TPrimaryKey> AttachGraphAndGetIdAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>, new()
        {
            return Task.FromResult(AttachGraphAndGetId(repository, entity));
        }
    }
}