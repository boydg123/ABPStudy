namespace Abp.Events.Bus.Handlers
{
    /// <summary>
    /// Undirect base interface for all event handlers.Implement <see cref="IEventHandler{TEventData}"/> instead of this one.
    /// (所有事件处理程序的间接基类接口)不要直接实现此接口，实现<see cref="IEventHandler{TEventData}"/>接口来代替
    /// </summary>
    public interface IEventHandler
    {
        
    }
}