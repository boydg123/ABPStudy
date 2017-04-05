using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Uow;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// 所有的仓储必须实现此接口，以确保实现了接口中的方法
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on / 仓储负责的主要实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity / 实体的主键类型</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region  查询 Select/Get/Query

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// 要使用此方法必须使用<see cref="UnitOfWorkAttribute"/> 特性，因为此操作需要使用工作单元，打开数据库连接
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database / 用于从数据库中获取实体的IQueryable</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.One or more
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions. / 表达式集合</param>
        /// <returns>IQueryable to be used to select entities from database / 用于从数据库中获取实体的IQueryable</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get all entities.
        /// 获取所有实体
        /// </summary>
        /// <returns>List of all entities / 实体列表</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// Used to get all entities. 
        /// 异步获取所有实体
        /// </summary>
        /// <returns>List of all entities / 实体列表</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// 基于给定表达式 <see cref="predicate"/> 来获取所有实体
        /// </summary>
        /// <param name="predicate">A condition to filter entities / 单个查询实体条件</param>
        /// <returns>List of all entities / 实体列表</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// 基于给定表达式 <see cref="predicate"/> 来获取所有实体 - 异步
        /// </summary>
        /// <param name="predicate">A condition to filter entities / 单个查询实体条件</param>
        /// <returns>List of all entities / 实体列表</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to run a query over entire entities.
        /// 用于在整个实体集合中执行一个查询
        /// <see cref="UnitOfWorkAttribute"/> attribute is not always necessary (as opposite to <see cref="GetAll"/>)
        /// 此操作不一定需要特性<see cref="UnitOfWorkAttribute"/> (与<see cref="GetAll"/>操作相反)
        /// if <paramref name="queryMethod"/> finishes IQueryable with ToList, FirstOrDefault etc..
        /// 除非 <paramref name="queryMethod"/> 使用 ToList, FirstOrDefault 等等
        /// </summary>
        /// <typeparam name="T">Type of return value of this method / 返回的类型</typeparam>
        /// <param name="queryMethod">This method is used to query over entities /查询实体的方法</param>
        /// <returns>Query result / 查询结果</returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// Gets an entity with given primary key.
        /// 通过主键获取实体
        /// </summary>
        /// <param name="id">Primary key of the entity to get / 给定的主键</param>
        /// <returns>Entity / 实体</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// 通过主键获取实体 - 异步
        /// </summary>
        /// <param name="id">Primary key of the entity to get / 给定的主键</param>
        /// <returns>Entity / 实体</returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// Gets exactly one entity with given predicate.Throws exception if no entity or more than one entity.
        /// 通过表达式获取一个确切的实体，如果实体不存在或者多个实体则抛出异常
        /// </summary>
        /// <param name="predicate">Entity / 表达式</param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets exactly one entity with given predicate.Throws exception if no entity or more than one entity.
        /// 通过表达式获取一个确切的实体，如果实体不存在或者多个实体则抛出异常
        /// </summary>
        /// <param name="predicate">Entity / 表达式</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// 通过主键获取实体，如果没有找到则返回Null
        /// </summary>
        /// <param name="id">Primary key of the entity to get / 实体主键</param>
        /// <returns>Entity or null / 实体或者Null</returns>
        TEntity FirstOrDefault(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// 通过主键获取实体，如果没有找到则返回Null - 异步
        /// </summary>
        /// <param name="id">Primary key of the entity to get / 实体主键</param>
        /// <returns>Entity or null / 实体或者Null</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// 通过给定的表达式获取单个实体，如果没找到则返回Null
        /// </summary>
        /// <param name="predicate">Predicate to filter entities / 查询实体的表达式条件</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// 通过给定的表达式获取单个实体，如果没找到则返回Null - 异步
        /// </summary>
        /// <param name="predicate">Predicate to filter entities / 查询实体的表达式条件</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Creates an entity with given primary key without database access.
        /// 通过给定的主键获取单个实体，不用反问数据库
        /// </summary>
        /// <param name="id">Primary key of the entity to load / 实体主键</param>
        /// <returns>Entity / 实体</returns>
        TEntity Load(TPrimaryKey id);

        #endregion

        #region 插入 Insert

        /// <summary>
        /// Inserts a new entity.
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">Inserted entity / 实体</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// 插入一个新的实体 - 异步
        /// </summary>
        /// <param name="entity">Inserted entity / 实体</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.It may require to save current unit of work to be able to retrieve id.
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        /// <returns>Id of the entity / 实体的ID</returns>
        TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.It may require to save current unit of work to be able to retrieve id.
        /// 插入实体获取它的ID，它可能需要保存当前的工作单元以便得到ID - 异步
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        /// <returns>Id of the entity / 实体的ID</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// 根据实体的ID值来插入或者更新实体
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// 根据实体的ID值来插入或者更新实体 -  异步
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.It may require to save current unit of work to be able to retrieve id.
        /// 根据实体的ID值来插入或者更新实体
        /// 返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        /// <returns>Id of the entity / 实体ID</returns>
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.It may require to save current unit of work to be able to retrieve id.
        /// 根据实体的ID值来插入或者更新实体 - 异步
        /// 返回实体的ID，它可能需要保存当前的工作单元以便得到ID
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        /// <returns>Id of the entity / 实体的ID</returns>
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion

        #region 更新 Update

        /// <summary>
        /// Updates an existing entity. 
        /// 更新已存在的实体
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// 更新已存在的实体 - 异步
        /// </summary>
        /// <param name="entity">Entity / 实体</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity.
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="id">Id of the entity / 实体的ID</param>
        /// <param name="updateAction">Action that can be used to change values of the entity / 用来修改实体值的方法</param>
        /// <returns>Updated entity / 被更改的实体</returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        /// Updates an existing entity.
        /// 更新一个已存在的实体 - 异步
        /// </summary>
        /// <param name="id">Id of the entity / 实体的ID</param>
        /// <param name="updateAction">Action that can be used to change values of the entity / 用来修改实体值的方法</param>
        /// <returns>Updated entity / 被更改的实体</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        #endregion

        #region 删除 Delete

        /// <summary>
        /// Deletes an entity.
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">Entity to be deleted / 实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// 删除一个实体 - 异步
        /// </summary>
        /// <param name="entity">Entity to be deleted / 实体</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by primary key.
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id">Primary key of the entity / 实体的主键</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// Deletes an entity by primary key.
        /// 根据主键删除实体 - 异步
        /// </summary>
        /// <param name="id">Primary key of the entity / 实体主键</param>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// Deletes many entities by function.
        /// 通过表达式删除实体
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// 注意：满足条件的所有实体都将被获取并删除
        /// This may cause major performance problems if there are too many entities with given predicate.
        /// 如果删除的实体数量过多，这可能会引起较大性能问题
        /// </summary>
        /// <param name="predicate">A condition to filter entities / 过滤实体的表达式</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes many entities by function.
        /// 通过表达式删除实体 - 异步
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// 注意：满足条件的所有实体都将被获取并删除
        /// This may cause major performance problems if there are too many entities with given predicate.
        /// 如果删除的实体数量过多，这可能会引起较大性能问题
        /// </summary>
        /// <param name="predicate">A condition to filter entities / 过滤实体的表达式</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 聚合 Aggregates

        /// <summary>
        /// Gets count of all entities in this repository.
        /// 获取当前仓储下所有实体的数量
        /// </summary>
        /// <returns>Count of entities / 实体数量</returns>
        int Count();

        /// <summary>
        /// Gets count of all entities in this repository.
        /// 获取当前仓储下所有实体的数量 - 异步
        /// </summary>
        /// <returns>Count of entities / 实体数量</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量
        /// </summary>
        /// <param name="predicate">A method to filter count / 一个过滤实体的方法</param>
        /// <returns>Count of entities / 实体数量</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量 - 异步
        /// </summary>
        /// <param name="predicate">A method to filter count / 一个过滤实体的方法</param>
        /// <returns>Count of entities / 实体数量</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greather than <see cref="int.MaxValue"/>.
        /// 获取此仓储下所有实体的数量 (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <returns>Count of entities / 实体数量</returns>
        long LongCount();

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greather than <see cref="int.MaxValue"/>.
        /// 获取此仓储下所有实体的数量 (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <returns>Count of entities / 实体数量</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量
        /// (use this overload if expected return value is greather than <see cref="int.MaxValue"/>).
        /// (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <param name="predicate">A method to filter count / 一个过滤实体的方法</param>
        /// <returns>Count of entities / 实体数量</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// 获取此仓储下满足给定条件 <paramref name="predicate"/>的实体数量 - 异步
        /// (use this overload if expected return value is greather than <see cref="int.MaxValue"/>).
        /// (如果返回值可能会超过<see cref="int.MaxValue"/>.请使用此方法)
        /// </summary>
        /// <param name="predicate">A method to filter count / 一个过滤实体的方法</param>
        /// <returns>Count of entities / 实体数量</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion
    }
}
