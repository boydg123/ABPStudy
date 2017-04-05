namespace Abp.Threading.BackgroundWorkers
{
    /// <summary>
    /// Used to manage background workers.
    /// 后台工作者管理类
    /// </summary>
    public interface IBackgroundWorkerManager : IRunnable
    {
        /// <summary>
        /// Adds a new worker. Starts the worker immediately if <see cref="IBackgroundWorkerManager"/> has started.
        /// 添加新的工作者。立即开始工作如果<see cref="IBackgroundWorkerManager"/>已经启动。
        /// </summary>
        /// <param name="worker">
        /// The worker. It should be resolved from IOC.
        /// 后台工作者，它必须从IOC中解析
        /// </param>
        void Add(IBackgroundWorker worker);
    }
}