using System;
using Castle.Windsor;

namespace Abp.Dependency
{
    /// <summary>
    /// This interface is used to directly perform dependency injection tasks.
    /// 此接口用于直接执行依赖注入任务
    /// </summary>
    public interface IIocManager : IIocRegistrar, IIocResolver, IDisposable
    {
        /// <summary>
        /// Reference to the Castle Windsor Container.
        /// Castle Windsor容器的引用
        /// </summary>
        IWindsorContainer IocContainer { get; }

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <param name="type">Type to check / 检查的类型</param>
        new bool IsRegistered(Type type);

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查给定的类型是否已经注册
        /// </summary>
        /// <typeparam name="T">Type to check / 检查的类型</typeparam>
        new bool IsRegistered<T>();
    }
}