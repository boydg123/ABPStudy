using System;

namespace Abp.Utils.Etc
{
    /// <summary>
    /// This class is used to simulate a Disposable that does nothing.
    /// 此用于来模拟可以销毁的对象，但它不做任何操作
    /// </summary>
    internal sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get { return SingletonInstance; } }
        private static readonly NullDisposable SingletonInstance = new NullDisposable();

        private NullDisposable()
        {
            
        }

        public void Dispose()
        {

        }
    }
}