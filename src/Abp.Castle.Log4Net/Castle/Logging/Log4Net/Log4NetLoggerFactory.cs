using System;
using Castle.Core.Logging;
using log4net;
using log4net.Config;

namespace Abp.Castle.Logging.Log4Net
{
    /// <summary>
    /// Log4Net日志工厂
    /// </summary>
    public class Log4NetLoggerFactory : AbstractLoggerFactory
    {
        /// <summary>
        /// 默认配置文件名
        /// </summary>
        public const string DefaultConfigFileName = "log4net.config";

        /// <summary>
        /// 构造函数
        /// </summary>
        public Log4NetLoggerFactory()
            : this(DefaultConfigFileName)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFileName">配置文件名</param>
        public Log4NetLoggerFactory(string configFileName)
        {
            var file = GetConfigFile(configFileName);
            XmlConfigurator.ConfigureAndWatch(file);
        }

        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="name">记录器名称</param>
        /// <returns>日志记录器</returns>
        public override ILogger Create(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new Log4NetLogger(LogManager.GetLogger(name), this);
        }

        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="name">记录器名称</param>
        /// <param name="level">日志记录严重程度</param>
        /// <returns>日志记录器</returns>
        public override ILogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("Logger levels cannot be set at runtime. Please review your configuration file.");
        }
    }
}