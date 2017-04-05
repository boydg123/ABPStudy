using System;

namespace Abp.Dependency
{
    /// <summary>
    /// This interface is used to wrap an object that is resolved from IOC container.
    /// 包装从IOC容器中解析出的对象。
    /// It inherits <see cref="IDisposable"/>, so resolved object can be easily released.
    /// 它继承至接口<see cref="IDisposable"/>, 因此，解析出来的对象，将很容易被释放。
    /// In <see cref="IDisposable.Dispose"/> method, <see cref="IIocResolver.Release"/> is called to dispose the object.
    /// 在方法<see cref="IDisposable.Dispose"/>中,将调用方法<see cref="IIocResolver.Release"/>销毁对象。
    /// This is non-generic version of <see cref="IDisposableDependencyObjectWrapper{T}"/> interface.
    /// 此类是接口<see cref="IDisposableDependencyObjectWrapper{T}"/> 的非泛型版本.
    /// </summary>
    public interface IDisposableDependencyObjectWrapper : IDisposableDependencyObjectWrapper<object>
    {

    }
}