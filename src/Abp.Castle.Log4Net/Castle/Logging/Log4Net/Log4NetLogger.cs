using System;
using System.Globalization;
using log4net;
using log4net.Core;
using log4net.Util;
using ILogger = Castle.Core.Logging.ILogger;

namespace Abp.Castle.Logging.Log4Net
{
    /// <summary>
    /// Log4Net日志记录器
    /// </summary>
    [Serializable]
    public class Log4NetLogger : MarshalByRefObject, ILogger
    {
        /// <summary>
        /// 日志记录器声明类型
        /// </summary>
        private static readonly Type DeclaringType = typeof(Log4NetLogger);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="factory">Log4Net日志工厂</param>
        public Log4NetLogger(log4net.Core.ILogger logger, Log4NetLoggerFactory factory)
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Log4NetLogger()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="log">日志记录接口</param>
        /// <param name="factory">Log4Net日志工厂</param>
        public Log4NetLogger(ILog log, Log4NetLoggerFactory factory)
            : this(log.Logger, factory)
        {
        }

        /// <summary>
        /// Debug是否可用
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return Logger.IsEnabledFor(Level.Debug); }
        }

        /// <summary>
        /// Error是否可用
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return Logger.IsEnabledFor(Level.Error); }
        }

        /// <summary>
        /// Fatal是否可用
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return Logger.IsEnabledFor(Level.Fatal); }
        }

        /// <summary>
        /// Info是否可用
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return Logger.IsEnabledFor(Level.Info); }
        }

        /// <summary>
        /// Warn是否可用
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return Logger.IsEnabledFor(Level.Warn); }
        }

        /// <summary>
        /// Log4Net日志工厂
        /// </summary>
        protected internal Log4NetLoggerFactory Factory { get; set; }

        /// <summary>
        /// Log4Net日志记录器
        /// </summary>
        protected internal log4net.Core.ILogger Logger { get; set; }

        public override string ToString()
        {
            return Logger.ToString();
        }

        /// <summary>
        /// 创建子日志记录器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual global::Castle.Core.Logging.ILogger CreateChildLogger(string name)
        {
            return Factory.Create(Logger.Name + "." + name);
        }

        /// <summary>
        /// 写入Debug消息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, message, null);
            }
        }

        /// <summary>
        /// 写入Debug消息日志
        /// </summary>
        /// <param name="messageFactory">写消息的方法</param>
        public void Debug(Func<string> messageFactory)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, messageFactory.Invoke(), null);
            }
        }

        /// <summary>
        /// 写入Debug消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, message, exception);
            }
        }

        /// <summary>
        /// 写入格式化Debug消息日志
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void DebugFormat(string format, params Object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Debug消息日志
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void DebugFormat(Exception exception, string format, params Object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
            }
        }

        /// <summary>
        /// 写入格式化Debug消息日志
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void DebugFormat(IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, new SystemStringFormat(formatProvider, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Debug消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsDebugEnabled)
            {
                Logger.Log(DeclaringType, Level.Debug, new SystemStringFormat(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        /// 写入Error消息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Error(string message)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, message, null);
            }
        }

        /// <summary>
        /// 写入Error消息日志
        /// </summary>
        /// <param name="messageFactory">写消息的方法</param>
        public void Error(Func<string> messageFactory)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, messageFactory.Invoke(), null);
            }
        }

        /// <summary>
        /// 写入Error消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, message, exception);
            }
        }

        /// <summary>
        /// 写入格式化Error消息日志
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void ErrorFormat(string format, params Object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Error消息日志
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数</param>
        public void ErrorFormat(Exception exception, string format, params Object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
            }
        }

        /// <summary>
        /// 写入格式化Error消息日志
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数</param>
        public void ErrorFormat(IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, new SystemStringFormat(formatProvider, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Error消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数</param>
        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsErrorEnabled)
            {
                Logger.Log(DeclaringType, Level.Error, new SystemStringFormat(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        /// 写入Fatal消息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Fatal(string message)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, message, null);
            }
        }

        /// <summary>
        /// 写入Fatal消息日志
        /// </summary>
        /// <param name="messageFactory">写消息的方法</param>
        public void Fatal(Func<string> messageFactory)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, messageFactory.Invoke(), null);
            }
        }

        /// <summary>
        /// 写入Fatal消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        public void Fatal(string message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, message, exception);
            }
        }

        /// <summary>
        /// 写入格式化Fatal消息日志
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void FatalFormat(string format, params Object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Fatal消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void FatalFormat(Exception exception, string format, params Object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
            }
        }

        /// <summary>
        /// 写入格式化Fatal消息日志
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void FatalFormat(IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, new SystemStringFormat(formatProvider, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Fatal消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsFatalEnabled)
            {
                Logger.Log(DeclaringType, Level.Fatal, new SystemStringFormat(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        /// 写入Info消息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Info(string message)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, message, null);
            }
        }

        /// <summary>
        /// 写入Info消息日志
        /// </summary>
        /// <param name="messageFactory">写消息方法</param>
        public void Info(Func<string> messageFactory)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, messageFactory.Invoke(), null);
            }
        }

        /// <summary>
        /// 写入Info消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, message, exception);
            }
        }

        /// <summary>
        /// 写入格式化Info消息日志
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void InfoFormat(string format, params Object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Info消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void InfoFormat(Exception exception, string format, params Object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
            }
        }

        /// <summary>
        /// 写入格式化Info消息日志
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void InfoFormat(IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, new SystemStringFormat(formatProvider, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Info消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsInfoEnabled)
            {
                Logger.Log(DeclaringType, Level.Info, new SystemStringFormat(formatProvider, format, args), exception);
            }
        }

        /// <summary>
        /// 写入Warn消息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, message, null);
            }
        }

        /// <summary>
        /// 写入Warn消息日志
        /// </summary>
        /// <param name="messageFactory">写消息方法</param>
        public void Warn(Func<string> messageFactory)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, messageFactory.Invoke(), null);
            }
        }

        /// <summary>
        /// 写入Warn消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, message, exception);
            }
        }

        /// <summary>
        /// 写入格式化Warn消息日志
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void WarnFormat(string format, params Object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Warn消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void WarnFormat(Exception exception, string format, params Object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
            }
        }

        /// <summary>
        /// 写入格式化Warn消息日志
        /// </summary>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void WarnFormat(IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, new SystemStringFormat(formatProvider, format, args), null);
            }
        }

        /// <summary>
        /// 写入格式化Warn消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="formatProvider">格式化提供者</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params Object[] args)
        {
            if (IsWarnEnabled)
            {
                Logger.Log(DeclaringType, Level.Warn, new SystemStringFormat(formatProvider, format, args), exception);
            }
        }
    }
}