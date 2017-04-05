using System;
using Abp.Threading.Timers;

namespace Abp.Threading.BackgroundWorkers
{
    /// <summary>
    /// Extends <see cref="BackgroundWorkerBase"/> to add a periodic running Timer. 
    /// 添加周期运行计时器
    /// </summary>
    public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        /// <summary>
        /// ABP定时器
        /// </summary>
        protected readonly AbpTimer Timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicBackgroundWorkerBase"/> class.
        /// 初始化<see cref="PeriodicBackgroundWorkerBase"/>类的新实例
        /// </summary>
        /// <param name="timer">A timer.</param>
        protected PeriodicBackgroundWorkerBase(AbpTimer timer)
        {
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public override void Start()
        {
            base.Start();
            Timer.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Stop()
        {
            Timer.Stop();
            base.Stop();
        }

        /// <summary>
        /// 等待服务停止
        /// </summary>
        public override void WaitToStop()
        {
            Timer.WaitToStop();
            base.WaitToStop();
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer.
        /// 处理计时器过去的事件
        /// </summary>
        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Periodic works should be done by implementing this method.
        /// 定期工作应工作通过实现此方法
        /// </summary>
        protected abstract void DoWork();
    }
}