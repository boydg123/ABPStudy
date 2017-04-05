using System;
using System.Reflection;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Abp.Threading
{
    /// <summary>
    /// Provides some helper methods to work with async methods.
    /// 为异步方法调用提供一些辅助方法
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Checks if given method is an async method.
        /// 检测给定的方法是否是一个异步方法
        /// </summary>
        /// <param name="method">A method to check / 需要检测的方法</param>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        /// <summary>
        /// Runs a async method synchronously.
        /// 同步地运行一个异步方法.
        /// </summary>
        /// <param name="func">A function that returns a result / 返回一个结果的方法</param>
        /// <typeparam name="TResult">Result type / 返回结果的类型</typeparam>
        /// <returns>Result of the async operation / 返回异步操作的结果</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        /// <summary>
        /// Runs a async method synchronously.
        /// 同步地运行一个异步方法
        /// </summary>
        /// <param name="action">An async action / 一个异步委托</param>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
