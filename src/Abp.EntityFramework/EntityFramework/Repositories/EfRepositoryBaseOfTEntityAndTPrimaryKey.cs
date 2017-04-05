using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// EF实现仓储接口
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity"/>. / 包含<typeparamref name="TEntity"/>的数据库上下文</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository / 此仓储的实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity / 实体主键类型</typeparam>
    public class EfRepositoryBase<TDbContext, TEntity, TPrimaryKey> : AbpRepositoryBase<TEntity, TPrimaryKey>, IRepositoryWithDbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// 获取EF数据库上下文对象
        /// </summary>
        public virtual TDbContext Context { get { return _dbContextProvider.GetDbContext(MultiTenancySide); } }

        /// <summary>
        /// Gets DbSet for given entity.
        /// 为给定的实体获取DbSet
        /// </summary>
        public virtual DbSet<TEntity> Table { get { return Context.Set<TEntity>(); } }
        
        /// <summary>
        /// 数据库上下文提供者
        /// </summary>
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体,要使用此方法必须使用<see cref="UnitOfWorkAttribute"/> 特性，因为此操作需要使用工作单元，打开数据库连接
        /// </summary>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public override IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// </summary>
        /// <param name="propertySelectors">表达式集合</param>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return GetAll();
            }

            var query = GetAll();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        /// <summary>
        /// 异步获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        /// <summary>
        /// 基于给定表达式 <see cref="predicate"/> 来获取所有实体 - 异步
        /// </summary>
        /// <param name="predicate">查询实体的条件</param>
        /// <returns>实体列表</returns>
        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 通过表达式获取一个确切的实体，如果实体不存在或者多个实体则抛出异常 - 异步
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns>实体</returns>
        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        /// <summary>
        /// 通过主键获取实体，如果没有找到则返回Null - 异步
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体或者Null</returns>
        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 通过给定的表达式获取单个实体，如果没找到则返回Null - 异步
        /// </summary>
        /// <param name="predicate">查询实体的表达式条件</param>
        /// <returns>实体</returns>
        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity);
        }

        /// <summary>
        /// 插入一个新的实体 - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Table.Add(entity));
        }

        /// <summary>
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        /// <summary>
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体,返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体ID</returns>
        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体 - 异步
        /// 返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        /// <summary>
        /// 更新已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// 更新已存在的实体 - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">实体</param>
        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        public override void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = FirstOrDefault(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }

        /// <summary>
        /// 获取当前仓储下所有实体的数量 - 异步
        /// </summary>
        /// <returns>实体数量</returns>
        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns>实体数量</returns>
        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        /// <summary>
        /// 获取此仓储下所有实体的数量 (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <returns>实体数量</returns>
        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量 - 异步
        /// (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns></returns>
        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        /// <summary>
        /// 如果不存在则附件实体
        /// </summary>
        /// <param name="entity">要附件的实体</param>
        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns>数据库上下文</returns>
        public DbContext GetDbContext()
        {
            return Context;
        }
    }
}
