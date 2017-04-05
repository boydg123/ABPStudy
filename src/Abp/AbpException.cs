using System;
using System.Runtime.Serialization;

namespace Abp
{
    /// <summary>
    /// Base exception type for those are thrown by Abp system for Abp specific exceptions.
    /// abp系统抛出的特定异常的基类型
    /// </summary>
    [Serializable]
    public class AbpException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 创建一个样新的 <see cref="AbpException"/> 对象.
        /// </summary>
        public AbpException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 创建一个样新的 <see cref="AbpException"/> 对象.
        /// </summary>
        public AbpException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 创建一个样新的 <see cref="AbpException"/> 对象.
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public AbpException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 创建一个样新的 <see cref="AbpException"/> 对象.
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public AbpException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
