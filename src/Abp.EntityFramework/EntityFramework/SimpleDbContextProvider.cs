using System.Data.Entity;
using Abp.MultiTenancy;

namespace Abp.EntityFramework
{
    /// <summary>
    /// <see cref="IDbContextProvider{TDbContext}"/>简单实现
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 数据库上下文对象
        /// </summary>
        public TDbContext DbContext { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库上下文对象</param>
        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns>数据库上下文对象</returns>
        public TDbContext GetDbContext()
        {
            return DbContext;
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <param name="multiTenancySide">表示多租户双方中的一方</param>
        /// <returns>数据库上下文对象</returns>
        public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            return DbContext;
        }
    }
}