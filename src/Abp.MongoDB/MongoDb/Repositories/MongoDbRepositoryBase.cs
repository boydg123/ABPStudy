using System.Linq;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Abp.MongoDb.Repositories
{
    /// <summary>
    /// Implements IRepository for MongoDB.
    /// MongoDB仓储实现 - 主键为Int类型
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository / 仓储的实体类型</typeparam>
    public class MongoDbRepositoryBase<TEntity> : MongoDbRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">MongDB数据库提供者</param>
        public MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    /// <summary>
    /// Implements IRepository for MongoDB.
    /// MongoDB仓储实现
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class MongoDbRepositoryBase<TEntity, TPrimaryKey> : AbpRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// MongoDB数据库
        /// </summary>
        public virtual MongoDatabase Database
        {
            get { return _databaseProvider.Database; }
        }

        /// <summary>
        /// MongoDB数据库集合
        /// </summary>
        public virtual MongoCollection<TEntity> Collection
        {
            get
            {
                return _databaseProvider.Database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        /// <summary>
        /// MongoDB数据库提供者
        /// </summary>
        private readonly IMongoDatabaseProvider _databaseProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider"></param>
        public MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// 要使用此方法必须使用<see cref="UnitOfWorkAttribute"/> 特性，因为此操作需要使用工作单元，打开数据库连接
        /// </summary>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public override IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <param name="id">给定的主键</param>
        /// <returns>实体</returns>
        public override TEntity Get(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            var entity = Collection.FindOne(query);
            if (entity == null)
            {
                throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        /// <summary>
        /// 通过主键获取实体，如果没有找到则返回Null
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体或者Null</returns>
        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            return Collection.FindOne(query);
        }

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public override TEntity Insert(TEntity entity)
        {
            Collection.Insert(entity);
            return entity;
        }

        /// <summary>
        /// 更新已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public override TEntity Update(TEntity entity)
        {
            Collection.Save(entity);
            return entity;
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">实体</param>
        public override void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        public override void Delete(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            Collection.Remove(query);
        }
    }
}