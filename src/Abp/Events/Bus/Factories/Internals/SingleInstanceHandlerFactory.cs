using Abp.Events.Bus.Handlers;

namespace Abp.Events.Bus.Factories.Internals
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to handle events by a single instance object. 
    /// 这是<see cref="IEventHandlerFactory"/>的实现，通过单例对象来处理事件
    /// </summary>
    /// <remarks>
    /// This class always gets the same single instance of handler.
    /// 这个类总是获取相同的单例来处理事件
    /// </remarks>
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// The event handler instance.
        /// 事件处理器实例
        /// </summary>
        public IEventHandler HandlerInstance { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handler">事件处理器实例</param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        /// <summary>
        /// 获取一个事件处理器（函数）
        /// </summary>
        /// <returns>事件处理器（函数）</returns>
        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        /// <summary>
        /// 释放一个事件处理器（函数）
        /// </summary>
        /// <param name="handler">将被释放的处理器</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            
        }
    }
}