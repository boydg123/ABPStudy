using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using Abp.Dependency;
using Castle.Core;
using Castle.Core.Logging;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// CallContext implementation of <see cref="ICurrentUnitOfWorkProvider"/>. This is the default implementation.
    /// 调用<see cref="ICurrentUnitOfWorkProvider"/>上下文实现。这是一个默认的实现
    /// </summary>
    public class CallContextCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider, ITransientDependency
    {
        /// <summary>
        /// 消息日志对象
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        private const string ContextKey = "Abp.UnitOfWork.Current";

        /// <summary>
        /// 工作单元字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, IUnitOfWork> UnitOfWorkDictionary = new ConcurrentDictionary<string, IUnitOfWork>();

        /// <summary>
        /// 当前工作单元提供者的调用上下文
        /// </summary>
        public CallContextCurrentUnitOfWorkProvider()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 获取当前工作单元
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <returns></returns>
        private static IUnitOfWork GetCurrentUow(ILogger logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
            if (unitOfWorkKey == null)
            {
                return null;
            }

            IUnitOfWork unitOfWork;
            if (!UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on GetCurrentUow)! UnitOfWork key: " + unitOfWorkKey);
                CallContext.FreeNamedDataSlot(ContextKey);
                return null;
            }

            if (unitOfWork.IsDisposed)
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but the UOW was disposed!");
                UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                CallContext.FreeNamedDataSlot(ContextKey);
                return null;
            }

            return unitOfWork;
        }

        /// <summary>
        /// 设置工作单元
        /// </summary>
        /// <param name="value">工作单元</param>
        /// <param name="logger">日志记录器</param>
        private static void SetCurrentUow(IUnitOfWork value, ILogger logger)
        {
            if (value == null)
            {
                ExitFromCurrentUowScope(logger);
                return;
            }

            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
            if (unitOfWorkKey != null)
            {
                IUnitOfWork outer;
                if (UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out outer))
                {
                    if (outer == value)
                    {
                        logger.Warn("Setting the same UOW to the CallContext, no need to set again!");
                        return;
                    }

                    value.Outer = outer;
                }
                else
                {
                    //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on SetCurrentUow)! UnitOfWork key: " + unitOfWorkKey);
                }
            }

            unitOfWorkKey = value.Id;
            if (!UnitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
            {
                throw new AbpException("Can not set unit of work! UnitOfWorkDictionary.TryAdd returns false!");
            }

            CallContext.LogicalSetData(ContextKey, unitOfWorkKey);
        }

        /// <summary>
        /// 从当前工作单元域退出
        /// </summary>
        /// <param name="logger">日志记录器</param>
        private static void ExitFromCurrentUowScope(ILogger logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
            if (unitOfWorkKey == null)
            {
                logger.Warn("There is no current UOW to exit!");
                return;
            }

            IUnitOfWork unitOfWork;
            if (!UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on ExitFromCurrentUowScope)! UnitOfWork key: " + unitOfWorkKey);
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
            if (unitOfWork.Outer == null)
            {
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            //Restore outer UOW

            var outerUnitOfWorkKey = unitOfWork.Outer.Id;
            if (!UnitOfWorkDictionary.TryGetValue(outerUnitOfWorkKey, out unitOfWork))
            {
                //No outer UOW
                logger.Warn("Outer UOW key could not found in UnitOfWorkDictionary!");
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            CallContext.LogicalSetData(ContextKey, outerUnitOfWorkKey);
        }

        /// <summary>
        /// 获取/设置当前 <see cref="IUnitOfWork"/>.
        /// </summary>
        [DoNotWire]
        public IUnitOfWork Current
        {
            get { return GetCurrentUow(Logger); }
            set { SetCurrentUow(value, Logger); }
        }
    }
}