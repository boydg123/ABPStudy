using System;

namespace Abp.Events.Bus.Exceptions
{
    /// <summary>
    /// This type of events can be used to notify for an exception.
    /// 这个类型的事件可用于通知一个异常的产生
    /// </summary>
    public class ExceptionData : EventData
    {
        /// <summary>
        /// Exception object.
        /// 异常对象
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="exception">Exception object / 异常对象</param>
        public ExceptionData(Exception exception)
        {
            Exception = exception;
        }
    }
}
