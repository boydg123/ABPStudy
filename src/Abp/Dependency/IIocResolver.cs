using System;

namespace Abp.Dependency
{
    /// <summary>
    /// Define interface for classes those are used to resolve dependencies.
    /// 为需要使用依赖解析的类定义接口(依赖注入对象解析获取器)
    /// </summary>
    public interface IIocResolver
    {
        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 要获取对象的类型</typeparam>
        /// <returns>The object instance / 对象实例</returns>
        T Resolve<T>();

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to cast / 要获取对象的类型</typeparam>
        /// <param name="type">Type of the object to resolve / 要解决的对象类型</param>
        /// <returns>The object instance / 对象实例</returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 要获取对象的类型</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The object instance / 对象实例</returns>
        T Resolve<T>(object argumentsAsAnonymousType);

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the object to get / 要获取对象的类型</param>
        /// <returns>The object instance / 对象实例</returns>
        object Resolve(Type type);

        /// <summary>
        /// Gets an object from IOC container.Returning object must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the object to get / 要获取对象的类型</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The object instance / 对象实例</returns>
        object Resolve(Type type, object argumentsAsAnonymousType);

        /// <summary>
        /// Gets all implementations for given type.Returning objects must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the objects to resolve / 要解决的对象类型</typeparam>
        /// <returns>Object instances / 对象实例</returns>
        T[] ResolveAll<T>();

        /// <summary>
        /// Gets all implementations for given type.Returning objects must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <typeparam name="T">Type of the objects to resolve / 要解决的对象类型</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>Object instances / 对象实例</returns>
        T[] ResolveAll<T>(object argumentsAsAnonymousType);

        /// <summary>
        /// Gets all implementations for given type.Returning objects must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the objects to resolve / 要解决的对象类型</param>
        /// <returns>Object instances / 对象实例</returns>
        object[] ResolveAll(Type type);

        /// <summary>
        /// Gets all implementations for given type.Returning objects must be Released (see <see cref="Release"/>) after usage.
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary> 
        /// <param name="type">Type of the objects to resolve / 要解决的对象类型</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>Object instances / objects</returns>
        object[] ResolveAll(Type type, object argumentsAsAnonymousType);

        /// <summary>
        /// Releases a pre-resolved object. See Resolve methods.
        /// 释放一个之前获取的对象，请查看方法Resolve.
        /// </summary>
        /// <param name="obj">Object to be released / 需要释放的对象</param>
        void Release(object obj);

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查类型是否已经注册
        /// </summary>
        /// <param name="type">Type to check / 需要检查的类型</param>
        bool IsRegistered(Type type);

        /// <summary>
        /// Checks whether given type is registered before.
        /// 检查类型是否已经注册
        /// </summary>
        /// <typeparam name="T">Type to check / 需要检查的类型</typeparam>
        bool IsRegistered<T>();
    }
}