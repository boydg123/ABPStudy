using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// Used to store information about an error.
    /// 用于存储错误相关信息
    /// </summary>
    [Serializable]
    public class ErrorInfo
    {
        /// <summary>
        /// Error code.
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// 错误详情
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// 如果存在错误，则验证错误
        /// </summary>
        public ValidationErrorInfo[] ValidationErrors { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        public ErrorInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="message">Error message / 错误消息</param>
        public ErrorInfo(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="code">Error code / 错误码</param>
        public ErrorInfo(int code)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="code">Error code / 错误码</param>
        /// <param name="message">Error message / 错误消息</param>
        public ErrorInfo(int code, string message)
            : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="message">Error message / 错误消息</param>
        /// <param name="details">Error details / 错误详情</param>
        public ErrorInfo(string message, string details)
            : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// 构造函数
        /// </summary>
        /// <param name="code">Error code / 错误码</param>
        /// <param name="message">Error message / 错误消息</param>
        /// <param name="details">Error details / 错误详情</param>
        public ErrorInfo(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }
    }
}