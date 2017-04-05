using System;

namespace Abp.Threading.Extensions
{
    /// <summary>
    /// Extension methods to make locking easier.
    /// 使lock更简单的扩展方法
    /// </summary>
    public static class LockExtensions
    {
        /// <summary>
        /// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
        /// 通过锁定给定的<see cref="source"/>对象.执行给定的<see cref="action"/>
        /// </summary>
        /// <param name="source">Source object (to be locked) / 源对象(将被锁定)</param>
        /// <param name="action">Action (to be executed) / Action (将被执行)</param>
        public static void Locking(this object source, Action action)
        {
            lock (source)
            {
                action();
            }
        }

        /// <summary>
        /// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
        /// 通过锁定给定的<see cref="source"/>对象.执行给定的<see cref="action"/> 
        /// </summary>
        /// <typeparam name="T">Type of the object (to be locked) / 对象的类型 (将被锁定)</typeparam>
        /// <param name="source">Source object (to be locked) / 源对象(将被锁定)</param>
        /// <param name="action">Action (to be executed) / Action (将被执行)</param>
        public static void Locking<T>(this T source, Action<T> action) where T : class
        {
            lock (source)
            {
                action(source);
            }
        }

        /// <summary>
        /// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
        /// 通过锁定给定的<see cref="source"/>对象.执行给定的<see cref="func"/> 并返回它的返回值
        /// </summary>
        /// <typeparam name="TResult">Return type / 返回类型</typeparam>
        /// <param name="source">Source object (to be locked) / 源对象(将被锁定)</param>
        /// <param name="func">Function (to be executed) / Function (将被执行)</param>
        /// <returns>Return value of the <paramref name="func"/> / <paramref name="func"/>的返回值</returns>
        public static TResult Locking<TResult>(this object source, Func<TResult> func)
        {
            lock (source)
            {
                return func();
            }
        }

        /// <summary>
        /// Executes given <paramref name="func"/> and returns it's value by locking given <paramref name="source"/> object.
        /// 通过锁定给定的<see cref="source"/>对象.执行给定的<see cref="func"/> 并返回它的返回值
        /// </summary>
        /// <typeparam name="T">Type of the object (to be locked) / 对象的类型(将被锁定)</typeparam>
        /// <typeparam name="TResult">Return type / 返回类型</typeparam>
        /// <param name="source">Source object (to be locked) / 源对象(将被锁定)</param>
        /// <param name="func">Function (to be executed) / 方法(将被执行)</param>
        /// <returns>Return value of the <paramnref name="func"/> / <paramnref name="func"/>的返回值</returns>
        public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
        {
            lock (source)
            {
                return func(source);
            }
        }
    }
}
