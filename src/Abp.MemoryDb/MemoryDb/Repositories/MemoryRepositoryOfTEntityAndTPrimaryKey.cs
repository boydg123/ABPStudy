using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.MemoryDb.Repositories
{
    //TODO: Implement thread-safety..? 实现线程安全?
    /// <summary>
    /// 内存仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class MemoryRepository<TEntity, TPrimaryKey> : AbpRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 内存数据库
        /// </summary>
        public virtual MemoryDatabase Database { get { return _databaseProvider.Database; } }

        /// <summary>
        /// 实体Table
        /// </summary>
        public virtual List<TEntity> Table { get { return Database.Set<TEntity>(); } }

        /// <summary>
        /// 内存数据库提供者
        /// </summary>
        private readonly IMemoryDatabaseProvider _databaseProvider;

        /// <summary>
        /// 内存主键生成器
        /// </summary>
        private readonly MemoryPrimaryKeyGenerator<TPrimaryKey> _primaryKeyGenerator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider"></param>
        public MemoryRepository(IMemoryDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            _primaryKeyGenerator = new MemoryPrimaryKeyGenerator<TPrimaryKey>();
        }

        /// <summary>
        /// 获取一个IQueryalbe，它可以从整张表中获取实体
        /// 要使用此方法必须使用<see cref="UnitOfWorkAttribute"/> 特性，因为此操作需要使用工作单元，打开数据库连接
        /// </summary>
        /// <returns>用于从数据库中获取实体的IQueryable</returns>
        public override IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public override TEntity Insert(TEntity entity)
        {
            if (entity.IsTransient())
            {
                entity.Id = _primaryKeyGenerator.GetNext();
            }

            Table.Add(entity);
            return entity;
        }

        /// <summary>
        /// 更新已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public override TEntity Update(TEntity entity)
        {
            var index = Table.FindIndex(e => EqualityComparer<TPrimaryKey>.Default.Equals(e.Id, entity.Id));
            if (index >= 0)
            {
                Table[index] = entity;
            }

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
            var index = Table.FindIndex(e => EqualityComparer<TPrimaryKey>.Default.Equals(e.Id, id));
            if (index >= 0)
            {
                Table.RemoveAt(index);
            }
        }
    }
}