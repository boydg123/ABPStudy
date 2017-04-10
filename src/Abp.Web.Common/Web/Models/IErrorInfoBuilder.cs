using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// This interface is used to build <see cref="ErrorInfo"/> objects.
    /// 此接口用于构建<see cref="ErrorInfo"/>对象
    /// </summary>
    public interface IErrorInfoBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/> using the given <paramref name="exception"/> object.
        /// 使用给定的<paramref name="exception"/>对象创建一个<see cref="ErrorInfo"/>新的实例
        /// </summary>
        /// <param name="exception">The exception object / 异常对象</param>
        /// <returns>Created <see cref="ErrorInfo"/> object / 创建的<see cref="ErrorInfo"/>对象</returns>
        ErrorInfo BuildForException(Exception exception);

        /// <summary>
        /// Adds an <see cref="IExceptionToErrorInfoConverter"/> object.
        /// 添加一个<see cref="IExceptionToErrorInfoConverter"/>对象
        /// </summary>
        /// <param name="converter">Converter / 转换器</param>
        void AddExceptionConverter(IExceptionToErrorInfoConverter converter);
    }
}