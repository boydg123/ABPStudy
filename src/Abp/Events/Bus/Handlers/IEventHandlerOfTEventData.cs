namespace Abp.Events.Bus.Handlers
{
    /// <summary>
    /// Defines an interface of a class that handles events of type <see cref="TEventData"/>.
    /// 定义一个处理<see cref="TEventData"/>类型事件类的接口
    /// </summary>
    /// <typeparam name="TEventData">Event type to handle / 要处理的事件类型</typeparam>
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// 通过实现此方法，完成事件处理器处理事件
        /// </summary>
        /// <param name="eventData">Event data / 事件数据</param>
        void HandleEvent(TEventData eventData);
    }
}
