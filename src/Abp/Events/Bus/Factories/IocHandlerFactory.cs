using System;
using Abp.Dependency;
using Abp.Events.Bus.Handlers;

namespace Abp.Events.Bus.Factories
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to get/release handlers using Ioc.
    /// 此<see cref="IEventHandlerFactory"/>实现，使用Ioc获取或释放事件处理器
    /// </summary>
    public class IocHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// Type of the handler.
        /// 事件处理器类型
        /// </summary>
        public Type HandlerType { get; private set; }

        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// Creates a new instance of <see cref="IocHandlerFactory"/> class.
        /// 创建一个新的<see cref="IocHandlerFactory"/>对象.
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        /// <param name="handlerType">Type of the handler / 事件处理器类型</param>
        public IocHandlerFactory(IIocResolver iocResolver, Type handlerType)
        {
            _iocResolver = iocResolver;
            HandlerType = handlerType;
        }

        /// <summary>
        /// Resolves handler object from Ioc container.
        /// 从IOC容器中解析事件处理器对象
        /// </summary>
        /// <returns>Resolved handler object / 事件处理器对象</returns>
        public IEventHandler GetHandler()
        {
            return (IEventHandler)_iocResolver.Resolve(HandlerType);
        }

        /// <summary>
        /// Releases handler object using Ioc container.
        /// 使用IOC解析器释放事件处理器对象
        /// </summary>
        /// <param name="handler">Handler to be released / 事件处理器对象</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            _iocResolver.Release(handler);
        }
    }
}