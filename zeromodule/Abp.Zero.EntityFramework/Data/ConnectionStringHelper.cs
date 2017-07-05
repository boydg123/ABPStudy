using System.Configuration;

namespace Abp.Data
{
    /// <summary>
    /// 连接字符串帮助类
    /// </summary>
    public static class ConnectionStringHelper
    {
        /// <summary>
        /// Gets connection string from given connection string or name.
        /// 通过给定的Name或连接字符串得到连接字符串
        /// </summary>
        public static string GetConnectionString(string nameOrConnectionString)
        {
            var connStrSection = ConfigurationManager.ConnectionStrings[nameOrConnectionString];
            if (connStrSection != null)
            {
                return connStrSection.ConnectionString;
            }

            return nameOrConnectionString;
        }
    }
}
