using System;
using System.Threading;
using Abp.Dependency;

namespace Abp.Threading.Timers
{
    /// <summary>
    /// A roboust timer implementation that ensures no overlapping occurs. It waits exactly specified <see cref="Period"/> between ticks.
    /// ABP框架定时器
    /// </summary>
    //TODO: Extract interface or make all members virtual to make testing easier.
    public class AbpTimer : RunnableBase, ITransientDependency
    {
        /// <summary>
        /// This event is raised periodically according to Period of Timer.
        /// 根据计时器周期定期引起此事件
        /// </summary>
        public event EventHandler Elapsed;

        /// <summary>
        /// Task period of timer (as milliseconds).
        /// 计时器的任务周期(毫秒)
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Indicates whether timer raises Elapsed event on Start method of Timer for once.
        /// 计时器一次性启动方法时指示计时器是否已引发事件
        /// Default: False.
        /// </summary>
        public bool RunOnStart { get; set; }

        /// <summary>
        /// This timer is used to perfom the task at spesified intervals.
        /// 此计时器用来在指定间隔执行任务
        /// </summary>
        private readonly Timer _taskTimer;

        /// <summary>
        /// Indicates that whether timer is running or stopped.
        /// 指示计时器是否正在运行或停止
        /// </summary>
        private volatile bool _running;

        /// <summary>
        /// Indicates that whether performing the task or _taskTimer is in sleep mode.
        /// 指示是否执行任务或<see cref="_taskTimer"/>在睡眠模式
        /// This field is used to wait executing tasks when stopping Timer.
        /// 此字段用于在停止计时器时等待执行任务
        /// </summary>
        private volatile bool _performingTasks;

        /// <summary>
        /// Creates a new Timer.
        /// 构造函数(创建一个新的计时器)
        /// </summary>
        public AbpTimer()
        {
            _taskTimer = new Timer(TimerCallBack, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Creates a new Timer.
        /// 构造函数(创建一个新的计时器)
        /// </summary>
        /// <param name="period">Task period of timer (as milliseconds) / 计时器的任务周期(毫秒)</param>
        /// <param name="runOnStart">Indicates whether timer raises Elapsed event on Start method of Timer for once / 计时器一次性启动方法时指示计时器是否已引发事件</param>
        public AbpTimer(int period, bool runOnStart = false)
            : this()
        {
            Period = period;
            RunOnStart = runOnStart;
        }

        /// <summary>
        /// Starts the timer.
        /// 启动计时器
        /// </summary>
        public override void Start()
        {
            if (Period <= 0)
            {
                throw new AbpException("Period should be set before starting the timer!");
            }

            base.Start();

            _running = true;
            _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
        }

        /// <summary>
        /// Stops the timer.
        /// 停止计时器
        /// </summary>
        public override void Stop()
        {
            lock (_taskTimer)
            {
                _running = false;
                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            base.Stop();
        }

        /// <summary>
        /// Waits the service to stop.
        /// 等待服务停止
        /// </summary>
        public override void WaitToStop()
        {
            lock (_taskTimer)
            {
                while (_performingTasks)
                {
                    Monitor.Wait(_taskTimer);
                }
            }

            base.WaitToStop();
        }

        /// <summary>
        /// This method is called by _taskTimer.
        /// 这个方法被<see cref="_taskTimer"/>调用
        /// </summary>
        /// <param name="state">Not used argument / 不使用参数</param>
        private void TimerCallBack(object state)
        {
            lock (_taskTimer)
            {
                if (!_running || _performingTasks)
                {
                    return;
                }

                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _performingTasks = true;
            }

            try
            {
                if (Elapsed != null)
                {
                    Elapsed(this, new EventArgs());
                }
            }
            catch
            {

            }
            finally
            {
                lock (_taskTimer)
                {
                    _performingTasks = false;
                    if (_running)
                    {
                        _taskTimer.Change(Period, Timeout.Infinite);
                    }

                    Monitor.Pulse(_taskTimer);
                }
            }
        }
    }
}