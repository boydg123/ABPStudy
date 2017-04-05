using System;
using System.Threading.Tasks;
using Abp.Events.Bus.Factories;
using Abp.Events.Bus.Handlers;
using Abp.Utils.Etc;

namespace Abp.Events.Bus
{
    /// <summary>
    /// An event bus that implements Null object pattern.
    /// 事件总线的NULL对象模式实现
    /// </summary>
    public sealed class NullEventBus : IEventBus
    {
        /// <summary>
        /// Gets single instance of <see cref="NullEventBus"/> class.
        /// <see cref="NullEventBus"/>的单例.
        /// </summary>
        public static NullEventBus Instance { get { return SingletonInstance; } }
        private static readonly NullEventBus SingletonInstance = new NullEventBus();

        /// <summary>
        /// 构造函数
        /// </summary>
        private NullEventBus()
        {
        }

        /// <summary>
        /// 注册一个事件,事件发生时，给定的action会被调用
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="action">处理事件的Action</param>
        public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注册一个事件,事件发生时，给定的action会被调用
        /// </summary>
        /// <typeparam name="TEventData">事件数据类型</typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注册一个事件,事件发生时，一个新的 <see cref="THandler"/>实例对象被创建
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <typeparam name="THandler">事件处理器的类型</typeparam>
        /// <returns></returns>
        public IDisposable Register<TEventData, THandler>()
            where TEventData : IEventData
            where THandler : IEventHandler<TEventData>, new()
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注册一个事件,事件发生时，给定的相同的事件处理实例会被调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件处理器的对象</param>
        /// <returns></returns>
        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注册一个事件
        /// 给定的工厂被用来创建/释放事件处理器
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handlerFactory">用来创建/释放事件处理器的工厂</param>
        public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handlerFactory">用来创建/释放事件处理器的工厂</param>
        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            return NullDisposable.Instance;
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="action"></param>
        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handler">之前注册的事件处理器对象</param>
        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">之前注册的事件处理器对象</param>
        public void Unregister(Type eventType, IEventHandler handler)
        {
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="factory">之前注册的工厂对象</param>
        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
        {
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="factory">之前注册的工厂对象</param>
        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
        }

        /// <summary>
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
        }

        /// <summary>
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public void UnregisterAll(Type eventType)
        {
        }

        /// <summary>
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
        }

        /// <summary>
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
        }

        /// <summary>
        ///  触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger(Type eventType, IEventData eventData)
        {
        }

        /// <summary>
        ///  触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return new Task(() => { });
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            return new Task(() => { });
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            return new Task(() => { });
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            return new Task(() => { });
        }
    }
}
