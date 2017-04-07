using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.MemoryDb.Repositories
{
    /// <summary>
    /// 内存仓储 - 主键类型为Int
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MemoryRepository<TEntity> : MemoryRepository<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">内存数据库提供者</param>
        public MemoryRepository(IMemoryDatabaseProvider databaseProvider)
            : base(databaseProvider)
        {
        }
    }
}
