using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// Base class to implement <see cref="IRepository{TEntity,TPrimaryKey}"/>.It implements some methods in most simple way.
    /// 实现接口<see cref="IRepository{TEntity,TPrimaryKey}"/>的基类。它用最简单的方式实现了一些方法。
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository / 仓储处理实体的类型</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity / 实体主键类型</typeparam>
    public abstract class AbpRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region 属性
        /// <summary>
        /// The multi tenancy side / 租户双方
        /// </summary>
        public static MultiTenancySides? MultiTenancySide { get; private set; }

        /// <summary>
        /// 依赖注入对象解析获取器
        /// </summary>
        public IIocResolver IocResolver { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static AbpRepositoryBase()
        {
            var attr = typeof (TEntity).GetSingleAttributeOfTypeOrBaseTypesOrNull<MultiTenancySideAttribute>();
            if (attr != null)
            {
                MultiTenancySide = attr.Side;
            }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// 要使用此方法必须使用<see cref="UnitOfWorkAttribute"/> 特性，因为此操作需要使用工作单元，打开数据库连接
        /// </summary>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public abstract IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// </summary>
        /// <param name="propertySelectors">表达式集合</param>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAll();
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        /// <summary>
        /// 异步获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        /// <summary>
        /// 基于给定表达式 <see cref="predicate"/> 来获取所有实体
        /// </summary>
        /// <param name="predicate">查询实体的条件</param>
        /// <returns>实体列表</returns>
        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        /// <summary>
        /// 基于给定表达式 <see cref="predicate"/> 来获取所有实体 - 异步
        /// </summary>
        /// <param name="predicate">查询实体的条件</param>
        /// <returns>实体列表</returns>
        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        /// <summary>
        /// 用于在整个实体集合中执行一个查询,此操作不一定需要特性<see cref="UnitOfWorkAttribute"/> (与<see cref="GetAll"/>操作相反)
        /// 除非 <paramref name="queryMethod"/> 使用 ToList, FirstOrDefault 等等
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="queryMethod">查询实体的方法</param>
        /// <returns>查询结果</returns>
        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <param name="id">给定的主键</param>
        /// <returns>实体</returns>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        /// <summary>
        /// 通过主键获取实体 - 异步
        /// </summary>
        /// <param name="id">给定的主键</param>
        /// <returns>实体</returns>
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        /// <summary>
        /// 通过表达式获取一个确切的实体，如果实体不存在或者多个实体则抛出异常
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns>实体</returns>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        /// <summary>
        /// 通过表达式获取一个确切的实体，如果实体不存在或者多个实体则抛出异常 - 异步
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        /// <summary>
        /// 通过主键获取实体，如果没有找到则返回Null
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体或者Null</returns>
        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 通过主键获取实体，如果没有找到则返回Null - 异步
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体或者Null</returns>
        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        /// <summary>
        /// 通过给定的表达式获取单个实体，如果没找到则返回Null
        /// </summary>
        /// <param name="predicate">查询实体的表达式条件</param>
        /// <returns>实体</returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 通过给定的表达式获取单个实体，如果没找到则返回Null - 异步
        /// </summary>
        /// <param name="predicate">查询实体的表达式条件</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        /// <summary>
        /// 通过给定的主键获取单个实体，不用反问数据库
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体</returns>
        public virtual TEntity Load(TPrimaryKey id)
        {
            return Get(id);
        }
        #endregion

        #region 插入
        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public abstract TEntity Insert(TEntity entity);

        /// <summary>
        /// 插入一个新的实体 - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        /// <summary>
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).Id;
        }

        /// <summary>
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public virtual Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertAndGetId(entity));
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体 -  异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体,返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体ID</returns>
        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).Id;
        }

        /// <summary>
        /// 根据实体的ID值来插入或者更新实体 - 异步
        /// 返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的ID</returns>
        public virtual Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertOrUpdateAndGetId(entity));
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public abstract TEntity Update(TEntity entity);

        /// <summary>
        /// 更新已存在的实体 - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="id">实体的ID</param>
        /// <param name="updateAction">用来修改实体值的方法</param>
        /// <returns>被更改的实体</returns>
        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        /// <summary>
        /// 更新一个已存在的实体 - 异步
        /// </summary>
        /// <param name="id">实体的ID</param>
        /// <param name="updateAction">用来修改实体值的方法</param>
        /// <returns>被更改的实体</returns>
        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">实体</param>
        public abstract void Delete(TEntity entity);

        /// <summary>
        /// 删除一个实体 - 异步
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        public abstract void Delete(TPrimaryKey id);

        /// <summary>
        /// 根据主键删除实体 - 异步
        /// </summary>
        /// <param name="id">实体主键</param>
        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 通过表达式删除实体
        /// 注意：满足条件的所有实体都将被获取并删除,如果删除的实体数量过多，这可能会引起较大性能问题。
        /// </summary>
        /// <param name="predicate">过滤实体的表达式</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// 通过表达式删除实体 - 异步
        /// 注意：满足条件的所有实体都将被获取并删除,如果删除的实体数量过多，这可能会引起较大性能问题。
        /// </summary>
        /// <param name="predicate">过滤实体的表达式</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
        }
        #endregion

        #region 聚合
        /// <summary>
        /// 获取当前仓储下所有实体的数量
        /// </summary>
        /// <returns>实体数量</returns>
        public virtual int Count()
        {
            return GetAll().Count();
        }

        /// <summary>
        /// 获取当前仓储下所有实体的数量 - 异步
        /// </summary>
        /// <returns>实体数量</returns>
        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns>实体数量</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量 - 异步
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns>实体数量</returns>
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        /// <summary>
        /// 获取此仓储下所有实体的数量 (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <returns>实体数量</returns>
        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        /// <summary>
        /// 获取此仓储下所有实体的数量 (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <returns>实体数量</returns>
        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量
        /// (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns>实体数量</returns>
        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        /// <summary>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量 - 异步
        /// (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <param name="predicate">一个过滤实体的方法</param>
        /// <returns></returns>
        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }
        #endregion

        /// <summary>
        /// 为实体Id创建相同lambda表达式
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}