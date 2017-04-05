using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Events.Bus.Factories;
using Abp.Events.Bus.Handlers;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Abp.Events.Bus
{
    /// <summary>
    /// Installs event bus system and registers all handlers automatically.
    /// 安装事件总线系统并自动注册所有的事件处理函数
    /// </summary>
    internal class EventBusInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Ioc解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;
        /// <summary>
        /// 事件总线配置
        /// </summary>
        private readonly IEventBusConfiguration _eventBusConfiguration;
        /// <summary>
        /// 事件总线
        /// </summary>
        private IEventBus _eventBus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">Ioc解析器</param>
        public EventBusInstaller(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            _eventBusConfiguration = iocResolver.Resolve<IEventBusConfiguration>();
        }

        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="container">IOC容器</param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (_eventBusConfiguration.UseDefaultEventBus)
            {
                container.Register(
                    Component.For<IEventBus>().UsingFactoryMethod(() => EventBus.Default).LifestyleSingleton()
                    );
            }
            else
            {
                container.Register(
                    Component.For<IEventBus>().ImplementedBy<EventBus>().LifestyleSingleton()
                    );
            }

            _eventBus = container.Resolve<IEventBus>();

            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        /// <summary>
        /// 组件注册事件处理函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
             * gets all event handler interfaces and registers type to Event Bus for each handling event.
             */
            if (!typeof(IEventHandler).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    _eventBus.Register(genericArgs[0], new IocHandlerFactory(_iocResolver, handler.ComponentModel.Implementation));
                }
            }
        }
    }
}
