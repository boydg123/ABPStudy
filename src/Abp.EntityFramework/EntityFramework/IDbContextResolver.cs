namespace Abp.EntityFramework
{
    /// <summary>
    /// 数据库上下文解析器接口
    /// </summary>
    public interface IDbContextResolver
    {
        /// <summary>
        /// 解析数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库上下文</returns>
        TDbContext Resolve<TDbContext>(string connectionString);
    }
}