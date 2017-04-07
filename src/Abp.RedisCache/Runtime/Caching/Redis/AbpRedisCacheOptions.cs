using System.Configuration;
using Abp.Configuration.Startup;
using Abp.Extensions;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// ABP Redis缓存选项
    /// </summary>
    public class AbpRedisCacheOptions
    {
        /// <summary>
        /// ABP 启动配置
        /// </summary>
        public IAbpStartupConfiguration AbpStartupConfiguration { get; private set; }

        /// <summary>
        /// 连接字符串Key
        /// </summary>
        private const string ConnectionStringKey = "Abp.Redis.Cache";

        /// <summary>
        /// 数据库ID设置Key
        /// </summary>
        private const string DatabaseIdSettingKey = "Abp.Redis.Cache.DatabaseId";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库ID
        /// </summary>
        public int DatabaseId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpStartupConfiguration">ABP 启动配置</param>
        public AbpRedisCacheOptions(IAbpStartupConfiguration abpStartupConfiguration)
        {
            AbpStartupConfiguration = abpStartupConfiguration;

            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        /// <summary>
        /// 获取默认的数据库ID
        /// </summary>
        /// <returns></returns>
        private static int GetDefaultDatabaseId()
        {
            var appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            if (appSetting.IsNullOrEmpty())
            {
                return -1;
            }

            int databaseId;
            if (!int.TryParse(appSetting, out databaseId))
            {
                return -1;
            }

            return databaseId;
        }

        /// <summary>
        /// 获取默认的连接字符串
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultConnectionString()
        {
            var connStr = ConfigurationManager.ConnectionStrings[ConnectionStringKey];
            if (connStr == null || connStr.ConnectionString.IsNullOrWhiteSpace())
            {
                return "localhost";
            }

            return connStr.ConnectionString;
        }
    }
}