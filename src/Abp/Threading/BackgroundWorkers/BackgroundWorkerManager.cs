using System;
using System.Collections.Generic;
using Abp.Dependency;

namespace Abp.Threading.BackgroundWorkers
{
    /// <summary>
    /// Implements <see cref="IBackgroundWorkerManager"/>.
    /// <see cref="IBackgroundWorkerManager"/>的实现
    /// </summary>
    public class BackgroundWorkerManager : RunnableBase, IBackgroundWorkerManager, ISingletonDependency, IDisposable
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;
        
        /// <summary>
        /// 后台工作者列表
        /// </summary>
        private readonly List<IBackgroundWorker> _backgroundJobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
        /// 构造函数(初始化一个<see cref="BackgroundWorkerManager"/>类的新实例)
        /// </summary>
        public BackgroundWorkerManager(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            _backgroundJobs = new List<IBackgroundWorker>();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public override void Start()
        {
            base.Start();

            _backgroundJobs.ForEach(job => job.Start());
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Stop()
        {
            _backgroundJobs.ForEach(job => job.Stop());

            base.Stop();
        }

        /// <summary>
        /// 等待服务停止
        /// </summary>
        public override void WaitToStop()
        {
            _backgroundJobs.ForEach(job => job.WaitToStop());

            base.WaitToStop();
        }

        /// <summary>
        /// 添加一个后台工作者
        /// </summary>
        /// <param name="worker"></param>
        public void Add(IBackgroundWorker worker)
        {
            _backgroundJobs.Add(worker);

            if (IsRunning)
            {
                worker.Start();
            }
        }

        /// <summary>
        /// 是否释放
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _backgroundJobs.ForEach(_iocResolver.Release);
            _backgroundJobs.Clear();
        }
    }
}
