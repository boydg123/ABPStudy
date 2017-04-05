using System;

namespace Abp.Events.Bus.Exceptions
{
    /// <summary>
    /// This type of events are used to notify for exceptions handled by ABP infrastructure.
    /// 这个类型的事件用于通知一个异常已经被ABP框架处理
    /// </summary>
    public class AbpHandledExceptionData : ExceptionData
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="exception">Exception object / 异常对象</param>
        public AbpHandledExceptionData(Exception exception)
            : base(exception)
        {

        }
    }
}