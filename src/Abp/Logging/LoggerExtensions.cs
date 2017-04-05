using System;
using Castle.Core.Logging;

namespace Abp.Logging
{
    /// <summary>
    /// Extensions for <see cref="ILogger"/>.
    /// <see cref="ILogger"/>的扩展
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logger">记录日志的对象</param>
        /// <param name="severity">日志级别</param>
        /// <param name="message">日志信息</param>
        public static void Log(this ILogger logger, LogSeverity severity, string message)
        {
            switch (severity)
            {
                case LogSeverity.Fatal:
                    logger.Fatal(message);
                    break;
                case LogSeverity.Error:
                    logger.Error(message);
                    break;
                case LogSeverity.Warn:
                    logger.Warn(message);
                    break;
                case LogSeverity.Info:
                    logger.Info(message);
                    break;
                case LogSeverity.Debug:
                    logger.Debug(message);
                    break;
                default:
                    throw new AbpException("Unknown LogSeverity value: " + severity);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logger">记录日志的对象</param>
        /// <param name="severity">日志级别</param>
        /// <param name="message">日志信息</param>
        /// <param name="exception">异常对象</param>
        public static void Log(this ILogger logger, LogSeverity severity, string message, Exception exception)
        {
            switch (severity)
            {
                case LogSeverity.Fatal:
                    logger.Fatal(message, exception);
                    break;
                case LogSeverity.Error:
                    logger.Error(message, exception);
                    break;
                case LogSeverity.Warn:
                    logger.Warn(message, exception);
                    break;
                case LogSeverity.Info:
                    logger.Info(message, exception);
                    break;
                case LogSeverity.Debug:
                    logger.Debug(message, exception);
                    break;
                default:
                    throw new AbpException("Unknown LogSeverity value: " + severity);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logger">记录日志的对象</param>
        /// <param name="severity">日志级别</param>
        /// <param name="messageFactory">记录日志的方法</param>
        public static void Log(this ILogger logger, LogSeverity severity, Func<string> messageFactory)
        {
            switch (severity)
            {
                case LogSeverity.Fatal:
                    logger.Fatal(messageFactory);
                    break;
                case LogSeverity.Error:
                    logger.Error(messageFactory);
                    break;
                case LogSeverity.Warn:
                    logger.Warn(messageFactory);
                    break;
                case LogSeverity.Info:
                    logger.Info(messageFactory);
                    break;
                case LogSeverity.Debug:
                    logger.Debug(messageFactory);
                    break;
                default:
                    throw new AbpException("Unknown LogSeverity value: " + severity);
            }
        }
    }
}