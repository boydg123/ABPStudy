using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Abp.Events.Bus.Factories;
using Abp.Events.Bus.Factories.Internals;
using Abp.Events.Bus.Handlers;
using Abp.Events.Bus.Handlers.Internals;
using Abp.Extensions;
using Abp.Threading.Extensions;
using Castle.Core.Logging;

namespace Abp.Events.Bus
{
    /// <summary>
    /// Implements EventBus as Singleton pattern.
    /// 事件总线，以单例模式实现
    /// </summary>
    public class EventBus : IEventBus
    {
        #region 属性
        /// <summary>
        /// Gets the default <see cref="EventBus"/> instance.
        /// 获取默认的 <see cref="EventBus"/> 实例.
        /// </summary>
        public static EventBus Default { get; } = new EventBus();

        /// <summary>
        /// Reference to the Logger.
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region 私有字段
        /// <summary>
        /// All registered handler factories.
        /// 所有注册的处理器工厂
        /// Key: Type of the event Value: List of handler factories
        /// Key: 事件类型  Value: 处理器工厂列表
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;
        #endregion

        #region 构造函数
        /// <summary>
        /// Creates a new <see cref="EventBus"/> instance.
        /// 创建一个 <see cref="EventBus"/> 实例.
        /// Instead of creating a new instace, you can use <see cref="Default"/> to use Global <see cref="EventBus"/>.
        /// 创建一个实例, 你可以在全局访问 <see cref="Default"/> 来使用 <see cref="EventBus"/>.
        /// </summary>
        public EventBus()
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            Logger = NullLogger.Instance;
        }
        #endregion

        #region 公共方法

        #region 注册
        /// <summary>
        /// 注册一个事件
        /// 事件发生时，给定的action会被调用
        /// </summary>
        /// <param name="action">处理事件的Action</param>
        /// <typeparam name="TEventData">事件类型</typeparam>
        public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            return Register(typeof(TEventData), new ActionEventHandler<TEventData>(action));
        }

        /// <summary>
        /// 注册一个事件
        /// 事件发生时，给定的相同的事件处理实例会被调用
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handler">事件处理器的对象</param>
        public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            return Register(typeof(TEventData), handler);
        }

        /// <summary>
        ///  注册一个事件
        ///  事件发生时，一个新的 <see cref="THandler"/>实例对象被创建
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <typeparam name="THandler">事件处理器的类型</typeparam>
        public IDisposable Register<TEventData, THandler>()
            where TEventData : IEventData
            where THandler : IEventHandler<TEventData>, new()
        {
            return Register(typeof(TEventData), new TransientEventHandlerFactory<THandler>());
        }

        /// <summary>
        /// 注册一个事件
        /// 事件发生时，给定的相同的事件处理实例会被调用
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件处理器的对象</param>
        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return Register(eventType, new SingleInstanceHandlerFactory(handler));
        }

        /// <summary>
        /// 注册一个事件
        /// 给定的工厂被用来创建/释放事件处理器
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handlerFactory">用来创建/释放事件处理器的工厂</param>
        public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
        {
            return Register(typeof(TEventData), handlerFactory);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handlerFactory">用来创建/释放事件处理器的工厂</param>
        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories => factories.Add(handlerFactory));

            return new FactoryUnregistrar(this, eventType, handlerFactory);
        }
        #endregion

        #region 注销
        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="action"></param>
        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEventData))
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                        {
                            var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                            if (singleInstanceFactory == null)
                            {
                                return false;
                            }

                            var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEventData>;
                            if (actionHandler == null)
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
                });
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="handler">之前注册的事件处理器对象</param>
        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            Unregister(typeof(TEventData), handler);
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">之前注册的事件处理器对象</param>
        public void Unregister(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory &&
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                        );
                });
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="factory">之前注册的工厂对象</param>
        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
        {
            Unregister(typeof(TEventData), factory);
        }

        /// <summary>
        /// 注销一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="factory">之前注册的工厂对象</param>
        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        /// <summary>
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
            UnregisterAll(typeof(TEventData));
        }

        /// <summary>
        /// 注册给定类型的所有的事件处理器（函数）
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public void UnregisterAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        }
        #endregion

        #region 触发
        /// <summary>
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            Trigger((object)null, eventData);
        }

        /// <summary>
        /// 触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            Trigger(typeof(TEventData), eventSource, eventData);
        }

        /// <summary>
        ///  触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger(Type eventType, IEventData eventData)
        {
            Trigger(eventType, null, eventData);
        }

        /// <summary>
        ///  触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
            var exceptions = new List<Exception>();

            TriggerHandlingException(eventType, eventSource, eventData, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while triggering the event: " + eventType, exceptions);
            }
        }
        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 触发处理异常
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        /// <param name="exceptions">异常列表</param>
        private void TriggerHandlingException(Type eventType, object eventSource, IEventData eventData, List<Exception> exceptions)
        {
            //TODO: This method can be optimized by adding all possibilities to a dictionary.

            eventData.EventSource = eventSource;

            foreach (var handlerFactories in GetHandlerFactories(eventType))
            {
                foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
                {
                    var eventHandler = handlerFactory.GetHandler();

                    try
                    {
                        if (eventHandler == null)
                        {
                            throw new Exception($"Registered event handler for event type {handlerFactories.EventType.Name} does not implement IEventHandler<{handlerFactories.EventType.Name}> interface!");
                        }

                        var handlerType = typeof(IEventHandler<>).MakeGenericType(handlerFactories.EventType);

                        var method = handlerType.GetMethod(
                            "HandleEvent",
                            BindingFlags.Public | BindingFlags.Instance,
                            null,
                            new[] { handlerFactories.EventType },
                            null
                        );

                        method.Invoke(eventHandler, new object[] { eventData });
                    }
                    catch (TargetInvocationException ex)
                    {
                        exceptions.Add(ex.InnerException);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                    finally
                    {
                        handlerFactory.ReleaseHandler(eventHandler);
                    }
                }
            }

            //Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
            if (eventType.IsGenericType &&
                eventType.GetGenericArguments().Length == 1 &&
                typeof(IEventDataWithInheritableGenericArgument).IsAssignableFrom(eventType))
            {
                var genericArg = eventType.GetGenericArguments()[0];
                var baseArg = genericArg.BaseType;
                if (baseArg != null)
                {
                    var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
                    var constructorArgs = ((IEventDataWithInheritableGenericArgument) eventData).GetConstructorArgs();
                    var baseEventData = (IEventData) Activator.CreateInstance(baseEventType, constructorArgs);
                    baseEventData.EventTime = eventData.EventTime;
                    Trigger(baseEventType, eventData.EventSource, baseEventData);
                }
            }
        }

        /// <summary>
        /// 获取事件处理器工厂
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns></returns>
        private IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in _handlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        /// <summary>
        /// 应该触发处理程序的事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handlerType">处理器类型</param>
        /// <returns></returns>
        private static bool ShouldTriggerEventForHandler(Type eventType, Type handlerType)
        {
            //Should trigger same type
            if (handlerType == eventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerType.IsAssignableFrom(eventType))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 异步

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <typeparam name="TEventData">事件类型</typeparam>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return TriggerAsync((object)null, eventData);
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
            ExecutionContext.SuppressFlow();

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        Trigger(eventSource, eventData);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.ToString(), ex);
                    }
                });

            ExecutionContext.RestoreFlow();

            return task;
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns>处理异步操作的任务</returns>
        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            return TriggerAsync(eventType, null, eventData);
        }

        /// <summary>
        /// 异步触发发一个事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventSource">触发事件的对象（事件源）</param>
        /// <param name="eventData">与事件关联的数据</param>
        /// <returns></returns>
        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            ExecutionContext.SuppressFlow();

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        Trigger(eventType, eventSource, eventData);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.ToString(), ex);
                    }
                });

            ExecutionContext.RestoreFlow();

            return task;
        }

        /// <summary>
        /// 获取活创建事件处理工厂
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns></returns>
        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return _handlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        /// <summary>
        /// 事件处理工厂相关的事件类型
        /// </summary>
        private class EventTypeWithEventHandlerFactories
        {
            /// <summary>
            /// 事件列表
            /// </summary>
            public Type EventType { get; }

            /// <summary>
            /// 事件处理工厂列表
            /// </summary>
            public List<IEventHandlerFactory> EventHandlerFactories { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="eventType">事件列表</param>
            /// <param name="eventHandlerFactories">事件处理工厂列表</param>
            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }
        }
        #endregion
    }
}