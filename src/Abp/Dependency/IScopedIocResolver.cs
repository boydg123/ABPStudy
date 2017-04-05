using System;

namespace Abp.Dependency
{
    /// <summary>
    /// This interface is used to wrap a scope for batch resolvings in a single <c>using</c> statement.
    /// 这个接口用来在一个using语句中为批处理解析包装一个范围
    /// It inherits <see cref="IDisposable" /> and <see cref="IIocResolver" />, so resolved objects can be easily and batch manner released by IocResolver.
    /// 它继承自 <see cref="IDisposable" /> 和 <see cref="IIocResolver" />，所以可以通过IOC解析器批处理方式简单的解析对象。
    /// In <see cref="IDisposable.Dispose" /> method, <see cref="IIocResolver.Release" /> is called to dispose the object.
    /// 在<see cref="IDisposable.Dispose" />方法里，<see cref="IIocResolver.Release" />被用来释放对象
    /// </summary>
    public interface IScopedIocResolver : IIocResolver, IDisposable { }
}