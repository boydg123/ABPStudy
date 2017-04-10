using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// This interface can be implemented to convert an <see cref="Exception"/> object to an <see cref="ErrorInfo"/> object. 
    /// Implements Chain Of Responsibility pattern.
    /// 此接口能被实现，用于将<see cref="Exception"/>转换成<see cref="ErrorInfo"/>,实现责任链模式
    /// </summary>
    public interface IExceptionToErrorInfoConverter
    {
        /// <summary>
        /// Next converter. If this converter decide this exception is not known, it can call Next.Convert(...).
        /// 下一个转换器，如果此转换器解决此异常是未知的，它将调用Next.Convert()
        /// </summary>
        IExceptionToErrorInfoConverter Next { set; }

        /// <summary>
        /// Converter method.
        /// 转换方法
        /// </summary>
        /// <param name="exception">The exception / 异常对象</param>
        /// <returns>Error info or null / 错误消息或null</returns>
        ErrorInfo Convert(Exception exception);
    }
}