using System;
using Abp.Events.Bus.Handlers;

namespace Abp.Events.Bus.Factories.Internals
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to handle events by a single instance object. 
    /// 通过单独的实例对象来处理事件
    /// </summary>
    /// <remarks>
    /// This class always gets the same single instance of handler.
    /// 这个类总是获取一个新的的处理器实例
    /// </remarks>
    internal class TransientEventHandlerFactory<THandler> : IEventHandlerFactory 
        where THandler : IEventHandler, new()
    {
        /// <summary>
        /// Creates a new instance of the handler object.
        /// 创建一个事件处理器的实例
        /// </summary>
        /// <returns>The handler object / 事件处理器对象</returns>
        public IEventHandler GetHandler()
        {
            return new THandler();
        }

        /// <summary>
        /// Disposes the handler object if it's <see cref="IDisposable"/>. Does nothing if it's not.
        /// 如果事件处理器实现了 <see cref="IDisposable"/>，就销毁对象，如果没有，就什么也不做
        /// </summary>
        /// <param name="handler">Handler to be released / 要释放的事件处理器实例</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            if (handler is IDisposable)
            {
                (handler as IDisposable).Dispose();
            }
        }
    }
}