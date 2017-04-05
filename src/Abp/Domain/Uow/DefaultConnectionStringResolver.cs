using System;
using System.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Default implementation of <see cref="IConnectionStringResolver"/>.
    /// <see cref="IConnectionStringResolver"/> 默认实现
    /// Get connection string from <see cref="IAbpStartupConfiguration"/>,
    /// 从 <see cref="IAbpStartupConfiguration"/> 获取连接字符串
    /// or "Default" connection string in config file,or single connection string in config file.
    /// 要么是默认连接字符串，要么是单个连接字符串
    /// </summary>
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        /// <summary>
        /// ABP启动时默认配置信息
        /// </summary>
        private readonly IAbpStartupConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultConnectionStringResolver"/> class.
        /// 构造函数.初始化<see cref="DefaultConnectionStringResolver"/>类新的实例
        /// </summary>
        public DefaultConnectionStringResolver(IAbpStartupConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///  获取一个连接字符串名称(在配置文件) 或一个有效的连接字符串
        /// </summary>
        /// <param name="args">当解析连接字符串时使用的参数</param>
        /// <returns></returns>
        public virtual string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var defaultConnectionString = _configuration.DefaultNameOrConnectionString;
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }

            throw new AbpException("Could not find a connection string definition for the application. Set IAbpStartupConfiguration.DefaultNameOrConnectionString or add a 'Default' connection string to application .config file.");
        }
    }
}