namespace Abp.Threading
{
    /// <summary>
    /// Base implementation of <see cref="IRunnable"/>.
    /// <see cref="IRunnable"/>的基类实现
    /// </summary>
    public abstract class RunnableBase : IRunnable
    {
        /// <summary>
        /// A boolean value to control the running.
        /// 控制服务是否正在运行的bool值
        /// </summary>
        public bool IsRunning { get { return _isRunning; } }

        private volatile bool _isRunning;

        /// <summary>
        /// 启动服务
        /// </summary>
        public virtual void Start()
        {
            _isRunning = true;
        }

        /// <summary>
        /// 发送停止命令到服务。服务也许立即返回或停止异步,客户端应该调用<see cref="WaitToStop"/>方法来保证服务停止
        /// </summary>
        public virtual void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// 等待服务停止
        /// </summary>
        public virtual void WaitToStop()
        {

        }
    }
}