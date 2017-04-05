using System;
using System.Runtime.ExceptionServices;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// <see cref="Exception"/> 类扩展方法.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Uses <see cref="ExceptionDispatchInfo.Capture"/> method to re-throws exception while preserving stack trace.
        /// 使用方法 <see cref="ExceptionDispatchInfo.Capture"/> 重新抛出异常,保存堆栈跟踪，也就是原参数exception的堆栈信息，而不是重新抛出异常时的堆栈信息
        /// </summary>
        /// <param name="exception">Exception to be re-thrown / 要抛出的异常</param>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}