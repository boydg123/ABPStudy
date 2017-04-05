using System;
using System.Runtime.Serialization;
using Abp.Logging;

namespace Abp.Authorization
{
    /// <summary>
    /// This exception is thrown on an unauthorized request.
    /// 当一个请求未授权时抛出此异常
    /// </summary>
    [Serializable]
    public class AbpAuthorizationException : AbpException, IHasLogSeverity
    {
        /// <summary>
        /// Severity of the exception. Default: Warn.
        /// 异常严重程度。默认: 警告.
        /// </summary>
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// 创建一个新的 <see cref="AbpAuthorizationException"/> 对象 .
        /// </summary>
        public AbpAuthorizationException()
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// 创建一个新的 <see cref="AbpAuthorizationException"/> 对象 .
        /// </summary>
        public AbpAuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// 创建一个新的 <see cref="AbpAuthorizationException"/> 对象 .
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public AbpAuthorizationException(string message)
            : base(message)
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// 创建一个新的 <see cref="AbpAuthorizationException"/> 对象 .
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public AbpAuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = LogSeverity.Warn;
        }
    }
}