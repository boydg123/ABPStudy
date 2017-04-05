using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// This handle is used for innet unit of work scopes.
    /// 此处理用于内部的工作单元域
    /// A inner unit of work scope actually uses outer unit of work scope and has no effect on <see cref="IUnitOfWorkCompleteHandle.Complete"/> call.
    /// 一个内部的工作单元域可以使用外部工作单元域，调用<see cref="IUnitOfWorkCompleteHandle.Complete"/>
    /// But if it's not called, an exception is thrown at end of the UOW to rollback the UOW.
    /// 没有任何影响，但是，如果没有调用，将在工作单元结束时抛出常，并回滚
    /// 将修饰符internal 改为 public(Derrick 2017/04/02)
    /// </summary>
    public class InnerUnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// 消息
        /// </summary>
        public const string DidNotCallCompleteMethodExceptionMessage = "Did not call Complete method of a unit of work.";

        /// <summary>
        /// Compete是否已经调用
        /// </summary>
        private volatile bool _isCompleteCalled;
        /// <summary>
        /// 是否销毁
        /// </summary>
        private volatile bool _isDisposed;

        /// <summary>
        /// 完成工作单元
        /// </summary>
        public void Complete()
        {
            _isCompleteCalled = true;
        }

        /// <summary>
        /// 完成工作单元
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync()
        {
            _isCompleteCalled = true;           
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (!_isCompleteCalled)
            {
                if (HasException())
                {
                    return;
                }

                throw new AbpException(DidNotCallCompleteMethodExceptionMessage);
            }
        }

        /// <summary>
        /// 是否有异常
        /// </summary>
        /// <returns></returns>
        private static bool HasException()
        {
            try
            {
                return Marshal.GetExceptionCode() != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}