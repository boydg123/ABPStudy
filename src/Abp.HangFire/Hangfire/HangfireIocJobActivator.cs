using System;
using System.Collections.Generic;
using Abp.Dependency;
using Hangfire;

namespace Abp.Hangfire
{
    /// <summary>
    /// Hangfile-Job工作对象创建器
    /// </summary>
    public class HangfireIocJobActivator : JobActivator
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        public HangfireIocJobActivator(IIocResolver iocResolver)
        {
            if (iocResolver == null)
            {
                throw new ArgumentNullException(nameof(iocResolver));
            }

            _iocResolver = iocResolver;
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="jobType">Job类型</param>
        /// <returns></returns>
        public override object ActivateJob(Type jobType)
        {
            return _iocResolver.Resolve(jobType);
        }

        /// <summary>
        /// 开始一个生命周期作用域
        /// </summary>
        /// <param name="context">Job激活器上下文</param>
        /// <returns></returns>
        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new HangfireIocJobActivatorScope(this, _iocResolver);
        }

        /// <summary>
        /// Hangfire IOC Job 创建器作用于
        /// </summary>
        class HangfireIocJobActivatorScope : JobActivatorScope
        {
            /// <summary>
            /// Job创建器
            /// </summary>
            private readonly JobActivator _activator;

            /// <summary>
            /// IOC解析器
            /// </summary>
            private readonly IIocResolver _iocResolver;

            /// <summary>
            /// 待解析对象列表
            /// </summary>
            private readonly List<object> _resolvedObjects;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="activator">Job创建器</param>
            /// <param name="iocResolver">IOC解析器</param>
            public HangfireIocJobActivatorScope(JobActivator activator, IIocResolver iocResolver)
            {
                _activator = activator;
                _iocResolver = iocResolver;
                _resolvedObjects = new List<object>();
            }

            /// <summary>
            /// 解析对象
            /// </summary>
            /// <param name="type">待解析类型</param>
            /// <returns></returns>
            public override object Resolve(Type type)
            {
                var instance = _activator.ActivateJob(type);
                _resolvedObjects.Add(instance);
                return instance;
            }

            /// <summary>
            /// 释放作用域
            /// </summary>
            public override void DisposeScope()
            {
                _resolvedObjects.ForEach(_iocResolver.Release);
            }
        }
    }
}