using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Abp.Logging;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// This exception type is used to throws validation exceptions.
    /// 这个异常类型用于抛出验证异常
    /// </summary>
    [Serializable]
    public class AbpValidationException : AbpException, IHasLogSeverity
    {
        /// <summary>
        /// Detailed list of validation errors for this exception.
        /// 此异常包含的验证错误的详细信息列表
        /// </summary>
        public IList<ValidationResult> ValidationErrors { get; set; }

        /// <summary>
        /// Exception severity.Default: Warn.
        /// 异常日志严重程度
        /// </summary>
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public AbpValidationException()
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor for serializing.
        /// 为序列化构造函数
        /// </summary>
        public AbpValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public AbpValidationException(string message)
            : base(message)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = LogSeverity.Warn;
        }


        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="validationErrors">Validation errors / 验证结果集合</param>
        public AbpValidationException(string message, IList<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public AbpValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ValidationErrors = new List<ValidationResult>();
            Severity = LogSeverity.Warn;
        }
    }
}
