using System;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Used as event arguments on <see cref="IActiveUnitOfWork.Failed"/> event.
    /// 用于<see cref="IActiveUnitOfWork.Failed"/> 事件的参数.
    /// </summary>
    public class UnitOfWorkFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused failure.
        /// 引发失败的异常
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkFailedEventArgs"/> object.
        /// 创建一个新的<see cref="UnitOfWorkFailedEventArgs"/> 对象.
        /// </summary>
        /// <param name="exception">Exception that caused failure / 引发失败的异常</param>
        public UnitOfWorkFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
