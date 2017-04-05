using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Abp.Threading
{
    /// <summary>
    /// 内部异步辅助类
    /// </summary>
    internal static class InternalAsyncHelper
    {
        /// <summary>
        /// 等待任务完成，并在Finally块中执行action
        /// </summary>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task AwaitTaskWithFinally(Task actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// 等待Post方法任务完成，并在Finally块中执行action
        /// </summary>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="postAction">POST 方法</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// 等待任务(前一个Action,POST Action)完成，并在Finally块中执行action
        /// </summary>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="preAction">前一个方法</param>
        /// <param name="postAction">POST类型方法</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task AwaitTaskWithPreActionAndPostActionAndFinally(Func<Task> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction(exception);                    
                }
            }
        }

        /// <summary>
        /// 等待任务完成,并得到返回值,并在Finally块中执行action
        /// </summary>
        /// <typeparam name="T">任务对象</typeparam>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                return await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// 调用等待完成的任务,并得到返回值,并在Finally块中执行action
        /// </summary>
        /// <param name="taskReturnType">任务返回类型</param>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, finalAction });
        }

        /// <summary>
        /// 等待任务(POST Action)完成,并在Finally块中执行action,并且等待返回结果
        /// </summary>
        /// <typeparam name="T">任务对象</typeparam>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="postAction">POST类型的Action</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                var result = await actualReturnValue;
                await postAction();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// 调用等待任务(POST Action)完成,并在Finally块中执行action,并且等待返回结果
        /// </summary>
        /// <param name="taskReturnType">任务返回类型</param>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="action">等待执行的action</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
        {
            return typeof (InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }

        /// <summary>
        /// 等待任务(Pre Action 以及 Post Action)完成,并在Finally块中执行action,并且等待返回结果
        /// </summary>
        /// <typeparam name="T">任务对象</typeparam>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="preAction">Pre Action</param>
        /// <param name="postAction">Post Action</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static async Task<T> AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult<T>(Func<Task<T>> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                var result = await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();                    
                }

                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                if (finalAction != null)
                {
                    finalAction(exception);
                }
            }
        }

        /// <summary>
        /// 调用等待任务(Pre Action 以及 Post Action)完成,并在Finally块中执行action,并且等待返回结果
        /// </summary>
        /// <param name="taskReturnType">任务返回类型</param>
        /// <param name="actualReturnValue">实际返回值</param>
        /// <param name="preAction">Pre Action</param>
        /// <param name="postAction">Post Action</param>
        /// <param name="finalAction">最后执行的方法</param>
        /// <returns></returns>
        public static object CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(Type taskReturnType, Func<object> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, preAction, postAction, finalAction });
        }
    }
}