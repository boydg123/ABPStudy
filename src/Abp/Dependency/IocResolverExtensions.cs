using System;

namespace Abp.Dependency
{
    /// <summary>
    /// Extension methods to <see cref="IIocResolver"/> interface.
    /// 接口 <see cref="IIocResolver"/> 的扩展方法.
    /// </summary>
    public static class IocResolverExtensions
    {
        #region ResolveAsDisposable
        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/> / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, iocResolver.Resolve<T>());
        }

        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="type">Type of the object to resolve. This type must be convertible <typeparamref name="T"/>. / 解析对象的类型，它必须与<see cref="T"/>兼容.</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/> / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, Type type)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, (T)iocResolver.Resolve(type));
        }

        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="type">Type of the object to resolve. This type must be convertible to <see cref="IDisposable"/>. / 解析对象的类型，它必须与<see cref="T"/>兼容.</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/> / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocResolver iocResolver, Type type)
        {
            return new DisposableDependencyObjectWrapper(iocResolver, iocResolver.Resolve(type));
        }

        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/> / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, iocResolver.Resolve<T>(argumentsAsAnonymousType));
        }

        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <typeparam name="T">Type of the object to get / 获取对象的类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="type">Type of the object to resolve. This type must be convertible <typeparamref name="T"/>. / 解析对象的类型，它必须与<see cref="T"/>兼容.</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/> / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper<T> ResolveAsDisposable<T>(this IIocResolver iocResolver, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper<T>(iocResolver, (T)iocResolver.Resolve(type, argumentsAsAnonymousType)); 
        }

        /// <summary>
        /// Gets an <see cref="DisposableDependencyObjectWrapper{T}"/> object that wraps resolved object to be Disposable.
        /// 获取一个 <see cref="DisposableDependencyObjectWrapper{T}"/> 对象，它包装一个从IOC容器中解析出的对象，以便销毁对象。
        /// </summary> 
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="type">Type of the object to resolve. This type must be convertible to <see cref="IDisposable"/>. / 解析对象的类型，它必须与<see cref="DisposableDependencyObjectWrapper{T}"/>兼容.</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments / 构造函数参数</param>
        /// <returns>The instance object wrapped by <see cref="DisposableDependencyObjectWrapper{T}"/>  / 此实例被<see cref="DisposableDependencyObjectWrapper{T}"/>包装</returns>
        public static IDisposableDependencyObjectWrapper ResolveAsDisposable(this IIocResolver iocResolver, Type type, object argumentsAsAnonymousType)
        {
            return new DisposableDependencyObjectWrapper(iocResolver, iocResolver.Resolve(type, argumentsAsAnonymousType));
        }

        /// <summary>
        /// Gets a <see cref="ScopedIocResolver"/> object that starts a scope to resolved objects to be Disposable.
        /// 获取一个 <see cref="ScopedIocResolver"/> 对象启动一个范围，一次性解析对象
        /// </summary>
        /// <param name="iocResolver">解析器对象</param>
        /// <returns>The instance object wrapped by <see cref="ScopedIocResolver"/> / 此实例被<see cref="ScopedIocResolver"/>包装</returns>
        public static IScopedIocResolver CreateScope(this IIocResolver iocResolver)
        {
            return new ScopedIocResolver(iocResolver);
        }
        #endregion

        #region Using
        /// <summary>
        /// This method can be used to resolve and release an object automatically.You can use the object in <paramref name="action"/>.
        /// 此方法用于自动解析和释放一个对象。你可以在 <see cref="action"/>使用这个对象。
        /// </summary> 
        /// <typeparam name="T">Type of the object to use / 使用对象的类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="action">An action that can use the resolved object / 一个能使用此对象的action</param>
        public static void Using<T>(this IIocResolver iocResolver, Action<T> action)
        {
            using (var wrapper = iocResolver.ResolveAsDisposable<T>())
            {
                action(wrapper.Object);
            }
        }

        /// <summary>
        /// This method can be used to resolve and release an object automatically.You can use the object in <paramref name="func"/> and return a value.
        /// 此方法用于自动解析和释放一个对象。你可以在<paramref name="func"/>使用这个对象并返回一个值
        /// </summary> 
        /// <typeparam name="TService">Type of the service to use / 使用对象的类型</typeparam>
        /// <typeparam name="TReturn">Return type / 返回类型</typeparam>
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="func">A function that can use the resolved object / 一个能使用次对象的方法</param>
        public static TReturn Using<TService, TReturn>(this IIocResolver iocResolver, Func<TService, TReturn> func)
        {
            using (var obj = iocResolver.ResolveAsDisposable<TService>())
            {
                return func(obj.Object);
            }
        }

        /// <summary>
        /// This method starts a scope to resolve and release all objects automatically.You can use the <c>scope</c> in <see cref="action"/>.
        /// 此方法用于自动解析和释放一个对象。你可以在<see cref="action"/>使用scope
        /// </summary> 
        /// <param name="iocResolver">IIocResolver object / 解析器对象</param>
        /// <param name="action">An action that can use the resolved object / 一个能使用此对象的action</param>
        public static void UsingScope(this IIocResolver iocResolver, Action<IScopedIocResolver> action)
        {
            using (var scope = iocResolver.CreateScope())
            {
                action(scope);
            }
        }
        #endregion
    }
}
