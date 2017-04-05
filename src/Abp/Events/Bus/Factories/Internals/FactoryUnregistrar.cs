using System;

namespace Abp.Events.Bus.Factories.Internals
{
    /// <summary>
    /// Used to unregister a <see cref="IEventHandlerFactory"/> on <see cref="Dispose"/> method.
    /// 使用方法<see cref="Dispose"/>，注销<see cref="IEventHandlerFactory"/> 
    /// </summary>
    internal class FactoryUnregistrar : IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly Type _eventType;
        private readonly IEventHandlerFactory _factory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="eventType"></param>
        /// <param name="factory"></param>
        public FactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory)
        {
            _eventBus = eventBus;
            _eventType = eventType;
            _factory = factory;
        }

        /// <summary>
        /// 注销事件处理器
        /// </summary>
        public void Dispose()
        {
            _eventBus.Unregister(_eventType, _factory);
        }
    }
}