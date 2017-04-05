namespace Abp.Domain.Uow
{
    /// <summary>
    /// Used to get connection string when a database connection is needed.
    /// 当需要连接数据库的时候用来获取连接字符串
    /// </summary>
    public interface IConnectionStringResolver
    {
        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string.
        /// 获取一个连接字符串名称(在配置文件) 或一个有效的连接字符串
        /// </summary>
        /// <param name="args">Arguments that can be used while resolving connection string. / 当解析连接字符串时使用的参数</param>
        string GetNameOrConnectionString(ConnectionStringResolveArgs args);
    }
}