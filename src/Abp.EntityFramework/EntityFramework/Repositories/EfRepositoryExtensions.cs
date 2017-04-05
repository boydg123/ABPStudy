using System;
using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// EF仓储扩展
    /// </summary>
    public static class EfRepositoryExtensions
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="repository">仓储接口</param>
        /// <returns>数据库上下文</returns>
        public static DbContext GetDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var repositoryWithDbContext = repository as IRepositoryWithDbContext;
            if (repositoryWithDbContext == null)
            {
                throw new ArgumentException("Given repository does not implement IRepositoryWithDbContext", nameof(repository));
            }

            return repositoryWithDbContext.GetDbContext();
        }

        /// <summary>
        /// 从数据库上下文分离
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="entity">实体类型</param>
        public static void DetachFromDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            repository.GetDbContext().Entry(entity).State = EntityState.Detached;
        }
    }
}