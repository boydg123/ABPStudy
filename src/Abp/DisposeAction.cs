using System;

namespace Abp
{
    /// <summary>
    /// This class can be used to provide an action when Dipose method is called.
    /// 当前Dipose方法被调用时，这个类能提供一个action
    /// </summary>
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// 创建一个<see cref="DisposeAction"/> 对象.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed. / 当前Dipose方法被调用时，会执行此Action</param>
        public DisposeAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
