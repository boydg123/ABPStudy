using Abp.Events.Bus.Handlers;

namespace Abp.Events.Bus.Factories
{
    /// <summary>
    /// Defines an interface for factories those are responsible to create/get and release of event handlers.
    /// 为负责创建/获取和释放事件处理器的工厂定义接口
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets an event handler.
        /// 获取一个事件处理器（函数）
        /// </summary>
        /// <returns>The event handler / 事件处理器（函数）</returns>
        IEventHandler GetHandler();

        /// <summary>
        /// Releases an event handler.
        /// 释放一个事件处理器（函数）
        /// </summary>
        /// <param name="handler">Handle to be released / 将被释放的处理器</param>
        void ReleaseHandler(IEventHandler handler);
    }
}