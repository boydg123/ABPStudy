using System;
using System.Runtime.Serialization;
using Abp.Web.Models;

namespace Abp.WebApi.Client
{
    /// <summary>
    /// This exception is thrown when a remote method call made and remote application sent an error message.
    /// 当远程方法调用和远程应用程序发送错误消息时引发此异常
    /// </summary>
    [Serializable]
    public class AbpRemoteCallException : AbpException
    {
        /// <summary>
        /// Remote error information.
        /// 远程错误信息
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 构造函数
        /// </summary>
        public AbpRemoteCallException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 构造函数
        /// </summary>
        public AbpRemoteCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="errorInfo">Exception message / 异常消息</param>
        public AbpRemoteCallException(ErrorInfo errorInfo)
            : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
