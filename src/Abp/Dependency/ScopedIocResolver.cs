using System;
using System.Collections.Generic;
using System.Linq;

namespace Abp.Dependency
{
    /// <summary>
    /// IOC解析器作用域
    /// </summary>
    public class ScopedIocResolver : IScopedIocResolver
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;
        /// <summary>
        /// 解析对象
        /// </summary>
        private readonly List<object> _resolvedObjects;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        public ScopedIocResolver(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            _resolvedObjects = new List<object>();
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return Resolve<T>(typeof(T));
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <param name="type">要获取对象的类型</param>
        /// <returns></returns>
        public T Resolve<T>(Type type)
        {
            return (T)Resolve(type);
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <typeparam name="T">要获取的对象</typeparam>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns></returns>
        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return (T)Resolve(typeof(T), argumentsAsAnonymousType);
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <param name="type">要获取对象的类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return Resolve(type, null);
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <param name="type">要获取对象的类型</param>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns>对象实例</returns>
        public object Resolve(Type type, object argumentsAsAnonymousType)
        {
            var resolvedObject = argumentsAsAnonymousType != null
                ? _iocResolver.Resolve(type, argumentsAsAnonymousType)
                : _iocResolver.Resolve(type);

            _resolvedObjects.Add(resolvedObject);
            return resolvedObject;
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <typeparam name="T">要解决的对象</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ResolveAll(typeof(T)).OfType<T>().ToArray();
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <typeparam name="T">要解决的对象</typeparam>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns></returns>
        public T[] ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return ResolveAll(typeof(T), argumentsAsAnonymousType).OfType<T>().ToArray();
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <param name="type">要解决的对象类型</param>
        /// <returns></returns>
        public object[] ResolveAll(Type type)
        {
            return ResolveAll(type, null);
        }

        /// <summary>
        /// 从Ioc容器中获取一个对象.返回使用完必须释放(see <see cref="Release"/>) 的对象
        /// </summary>
        /// <param name="type">要解决的对象类型</param>
        /// <param name="argumentsAsAnonymousType">构造函数参数</param>
        /// <returns></returns>
        public object[] ResolveAll(Type type, object argumentsAsAnonymousType)
        {
            var resolvedObjects = argumentsAsAnonymousType != null
                ? _iocResolver.ResolveAll(type, argumentsAsAnonymousType)
                : _iocResolver.ResolveAll(type);

            _resolvedObjects.AddRange(resolvedObjects);
            return resolvedObjects;
        }

        /// <summary>
        /// 释放一个之前获取的对象，请查看方法Resolve.
        /// </summary>
        /// <param name="obj">需要释放的对象</param>
        public void Release(object obj)
        {
            _resolvedObjects.Remove(obj);
            _iocResolver.Release(obj);
        }

        /// <summary>
        /// 检查类型是否已经注册
        /// </summary>
        /// <param name="type">需要检查的类型</param>
        /// <returns></returns>
        public bool IsRegistered(Type type)
        {
            return _iocResolver.IsRegistered(type);
        }

        /// <summary>
        /// 检查类型是否已经注册
        /// </summary>
        /// <typeparam name="T">需要检查的对象</typeparam>
        /// <returns></returns>
        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        /// <summary>
        /// 释放一个之前获取的对象，请查看方法Resolve.
        /// </summary>
        public void Dispose()
        {
            _resolvedObjects.ForEach(_iocResolver.Release);
        }
    }
}
