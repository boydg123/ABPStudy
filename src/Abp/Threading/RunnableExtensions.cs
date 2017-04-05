namespace Abp.Threading
{
    /// <summary>
    /// Some extension methods for <see cref="IRunnable"/>.
    /// <see cref="IRunnable"/>的扩展方法
    /// </summary>
    public static class RunnableExtensions
    {
        /// <summary>
        /// Calls <see cref="IRunnable.Stop"/> and then <see cref="IRunnable.WaitToStop"/>.
        /// 调用<see cref="IRunnable.Stop"/>然后调用<see cref="IRunnable.WaitToStop"/>
        /// </summary>
        public static void StopAndWaitToStop(this IRunnable runnable)
        {
            runnable.Stop();
            runnable.WaitToStop();
        }
    }
}