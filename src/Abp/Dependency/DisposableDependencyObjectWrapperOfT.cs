namespace Abp.Dependency
{
    /// <summary>
    /// DisposableDependencyObjectWrapper{object}的简快捷类。
    /// 包装从IOC容器中解析出的对象。
    /// 它继承至接口<see cref="IDisposable"/>, 因此，解析出来的对象，将很容易被释放。
    /// 在方法<see cref="IDisposable.Dispose"/>中,将调用方法<see cref="IIocResolver.Release"/>销毁对象。
    /// </summary>
    internal class DisposableDependencyObjectWrapper : DisposableDependencyObjectWrapper<object>, IDisposableDependencyObjectWrapper
    {
        public DisposableDependencyObjectWrapper(IIocResolver iocResolver, object obj)
            : base(iocResolver, obj)
        {

        }
    }

    /// <summary>
    /// 包装从IOC容器中解析出的对象。
    /// 它继承至接口<see cref="IDisposable"/>, 因此，解析出来的对象，将很容易被释放。
    /// 在方法<see cref="IDisposable.Dispose"/>中,将调用方法<see cref="IIocResolver.Release"/>销毁对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DisposableDependencyObjectWrapper<T> : IDisposableDependencyObjectWrapper<T>
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 从IOC容器中解析出的对象
        /// </summary>
        public T Object { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        /// <param name="obj">从IOC容器中解析出的对象</param>
        public DisposableDependencyObjectWrapper(IIocResolver iocResolver, T obj)
        {
            _iocResolver = iocResolver;
            Object = obj;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            _iocResolver.Release(Object);
        }
    }
}