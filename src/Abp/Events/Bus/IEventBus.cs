using System;
using System.Threading.Tasks;
using Abp.Events.Bus.Factories;
using Abp.Events.Bus.Handlers;

namespace Abp.Events.Bus
{
    /// <summary>
    /// Defines interface of the event bus.
    /// 定义事件总线的接口
    /// </summary>
    public interface IEventBus
    {
        #region 注册 Register

        /// <summary>
        /// Registers to an event.Given action is called for all event occurrences.
        /// 注册一个事件，事件发生时，给定的action会被调用
        /// </summary>
        /// <param name="action">Action to handle events / 处理事件的Action</param>
        /// <typeparam name="TEventData">Event type / 事件数据类型</typeparam>
        IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// Registers to an event. Same (given) instance of the handler is used for all event occurrences.
        /// 注册一个事件，事件发生时，给定的相同的事件处理实例会被调用
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="handler">Object to handle the event / 事件处理器的对象</param>
        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// Registers to an event.A new instance of <see cref="THandler"/> object is created for every event occurrence.
        /// 注册一个事件,事件发生时，一个新的 <see cref="THandler"/>实例对象被创建
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <typeparam name="THandler">Type of the event handler / 事件处理器的类型</typeparam>
        IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new();

        /// <summary>
        /// Registers to an event.Same (given) instance of the handler is used for all event occurrences.
        /// 注册一个事件,事件发生时，给定的相同的事件处理实例会被调用
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="handler">Object to handle the event / 事件处理器的对象</param>
        IDisposable Register(Type eventType, IEventHandler handler);

        /// <summary>
        /// Registers to an event.Given factory is used to create/release handlers
        /// 注册一个事件,给定的工厂被用来创建/释放事件处理器
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="handlerFactory">A factory to create/release handlers / 用来创建/释放事件处理器的工厂</param>
        IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData;

        /// <summary>
        /// Registers to an event.
        /// 注册一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="handlerFactory">A factory to create/release handlers / 用来创建/释放事件处理器的工厂</param>
        IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

        #endregion

        #region 注销 Unregister

        /// <summary>
        /// Unregisters from an event.
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="action"></param>
        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="handler">Handler object that is registered before / 之前注册的事件处理器对象</param>
        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="handler">Handler object that is registered before / 之前注册的事件处理器对象</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        /// Unregisters from an event.
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="factory">Factory object that is registered before / 之前注册的工厂对象</param>
        void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;

        /// <summary>
        /// Unregisters from an event.
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="factory">Factory object that is registered before / 之前注册的工厂对象</param>
        void Unregister(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        void UnregisterAll<TEventData>() where TEventData : IEventData;

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        void UnregisterAll(Type eventType);

        #endregion

        #region Trigger

        /// <summary>
        /// Triggers an event.
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event.
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="eventSource">The object which triggers the event / 触发事件的对象（事件源）</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event.
        /// 触发发一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        void Trigger(Type eventType, IEventData eventData);

        /// <summary>
        /// Triggers an event.
        /// 触发发一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="eventSource">The object which triggers the event / 触发事件的对象（事件源）</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        void Trigger(Type eventType, object eventSource, IEventData eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// 异步触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="eventData">Related data for the even / 与事件关联的数据t</param>
        /// <returns>The task to handle async operation / 处理异步操作的任务</returns>
        Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event asynchronously.
        /// 异步触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">Event type / 事件类型</typeparam>
        /// <param name="eventSource">The object which triggers the event / 触发事件的对象（事件源）</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        /// <returns>The task to handle async operation / 处理异步操作的任务</returns>
        Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        /// <summary>
        /// Triggers an event asynchronously.
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        /// <returns>The task to handle async operation / 处理异步操作的任务</returns>
        Task TriggerAsync(Type eventType, IEventData eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">Event type / 事件类型</param>
        /// <param name="eventSource">The object which triggers the event / 触发事件的对象（事件源）</param>
        /// <param name="eventData">Related data for the event / 与事件关联的数据</param>
        /// <returns>The task to handle async operation / 处理异步操作的任务</returns>
        Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);


        #endregion
    }
}