using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.Linq
{
    /// <summary>
    /// This interface is intended to be used by ABP.
    /// 此接口的目的是要使用ABP(异步查询执行者)
    /// </summary>
    public interface IAsyncQueryableExecuter
    {
        /// <summary>
        /// 获取数量 - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns>查询的数量</returns>
        Task<int> CountAsync<T>(IQueryable<T> queryable);

        /// <summary>
        /// 转换成List - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        Task<List<T>> ToListAsync<T>(IQueryable<T> queryable);

        /// <summary>
        /// 获取默认或第一个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable);
    }
}