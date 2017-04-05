using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Linq;

namespace Abp.EntityFramework.Linq
{
    /// <summary>
    /// <see cref="IAsyncQueryableExecuter"/>实现，EF异步查询执行者
    /// </summary>
    public class EfAsyncQueryableExecuter : IAsyncQueryableExecuter, ISingletonDependency
    {
        /// <summary>
        /// 获取数量 - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns>查询的数量</returns>
        public Task<int> CountAsync<T>(IQueryable<T> queryable)
        {
            return queryable.CountAsync();
        }

        /// <summary>
        /// 转换成List - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        {
            return queryable.ToListAsync();
        }

        /// <summary>
        /// 获取默认或第一个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        {
            return queryable.FirstOrDefaultAsync();
        }
    }
}
