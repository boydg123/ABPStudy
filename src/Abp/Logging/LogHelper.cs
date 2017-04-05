using System;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Runtime.Validation;
using Castle.Core.Logging;

namespace Abp.Logging
{
    /// <summary>
    /// This class can be used to write logs from somewhere where it's a hard to get a reference to the <see cref="ILogger"/>.
    /// 这个类用于在很难获取<see cref="ILogger"/>引用的地方写日志
    /// Normally, use <see cref="ILogger"/> with property injection wherever it's possible.
    /// 一般情况，使用属性注入获取<see cref="ILogger"/>
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// A reference to the logger.
        /// logger的引用 
        /// </summary>
        public static ILogger Logger { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static LogHelper()
        {
            Logger = IocManager.Instance.IsRegistered(typeof(ILoggerFactory))
                ? IocManager.Instance.Resolve<ILoggerFactory>().Create(typeof(LogHelper))
                : NullLogger.Instance;
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex)
        {
            LogException(Logger, ex);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="logger">logger的引用</param>
        /// <param name="ex">异常对象</param>
        public static void LogException(ILogger logger, Exception ex)
        {
            var severity = (ex as IHasLogSeverity)?.Severity ?? LogSeverity.Error;

            logger.Log(severity, ex.Message, ex);

            LogValidationErrors(logger, ex);
        }

        /// <summary>
        /// 记录验证错误
        /// </summary>
        /// <param name="logger">logger的引用</param>
        /// <param name="exception">异常对象</param>
        private static void LogValidationErrors(ILogger logger, Exception exception)
        {
            //Try to find inner validation exception
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is AbpValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (!(exception is AbpValidationException))
            {
                return;
            }

            var validationException = exception as AbpValidationException;
            if (validationException.ValidationErrors.IsNullOrEmpty())
            {
                return;
            }

            logger.Log(validationException.Severity, "There are " + validationException.ValidationErrors.Count + " validation errors:");
            foreach (var validationResult in validationException.ValidationErrors)
            {
                var memberNames = "";
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                }

                logger.Log(validationException.Severity, validationResult.ErrorMessage + memberNames);
            }
        }
    }
}
