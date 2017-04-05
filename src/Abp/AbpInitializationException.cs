using System;
using System.Runtime.Serialization;

namespace Abp
{
    /// <summary>
    /// This exception is thrown if a problem on ABP initialization progress.
    /// 如果abp初始化过程发生问题，抛出此异常
    /// </summary>
    [Serializable]
    public class AbpInitializationException : AbpException
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public AbpInitializationException()
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// 构造函数（用于序列化）
        /// </summary>
        public AbpInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public AbpInitializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public AbpInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
