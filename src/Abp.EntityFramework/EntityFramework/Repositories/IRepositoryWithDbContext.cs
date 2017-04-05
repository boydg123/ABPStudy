using System.Data.Entity;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// 数据上下文仓储接口
    /// </summary>
    public interface IRepositoryWithDbContext
    {
        /// <summary>
        /// 获取数据库上下文对象
        /// </summary>
        /// <returns>数据库上下文</returns>
        DbContext GetDbContext();
    }
}