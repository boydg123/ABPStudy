using System;
using System.Runtime.Serialization;
using Abp.Logging;

namespace Abp.UI
{
    /// <summary>
    /// This exception type is directly shown to the user.
    /// 这个异常类型是直接向用户显示的
    /// </summary>
    [Serializable]
    public class UserFriendlyException : AbpException, IHasLogSeverity, IHasErrorCode
    {
        /// <summary>
        /// Additional information about the exception.
        /// 异常的额外信息
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// An arbitrary error code.
        /// 任意错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Severity of the exception.Default: Warn.
        /// 异常级别。默认:警告
        /// </summary>
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public UserFriendlyException()
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor for serializing.
        /// 构造函数-用于序列化
        /// </summary>
        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public UserFriendlyException(string message)
            : base(message)
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="severity">Exception severity / 异常级别</param>
        public UserFriendlyException(string message, LogSeverity severity)
            : base(message)
        {
            Severity = severity;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="code">Error code / 错误码</param>
        /// <param name="message">Exception message / 异常消息</param>
        public UserFriendlyException(int code, string message)
            : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="details">Additional information about the exception / 关于异常的附加信息</param>
        public UserFriendlyException(string message, string details)
            : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="code">Error code / 错误码</param>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="details">Additional information about the exception / 关于异常的附加信息</param>
        public UserFriendlyException(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="details">Additional information about the exception / 关于异常的附加信息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public UserFriendlyException(string message, string details, Exception innerException)
            : this(message, innerException)
        {
            Details = details;
        }
    }
}