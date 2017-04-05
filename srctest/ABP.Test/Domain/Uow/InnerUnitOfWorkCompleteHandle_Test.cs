using Abp;
using Abp.Domain.Uow;
using Shouldly;
using System;
using Xunit;

namespace ABP.Test.Domain.Uow
{
    /// <summary>
    /// 
    /// </summary>
    public class InnerUnitOfWorkCompleteHandle_Test
    {
        /// <summary>
        /// 完成方法被调用，不会抛异常
        /// </summary>
        [Fact]
        public void Should_Not_Throw_Exception_If_Complete_Called()
        {
            using (var uow = new InnerUnitOfWorkCompleteHandle())
            {
                uow.Complete();
            }
        }

        /// <summary>
        /// 完成方法没有被调用，会抛异常
        /// </summary>
        [Fact]
        public void Should_Throw_Exception_If_Complete_Did_Not_Called()
        {
            Assert.Throws<AbpException>(() =>
            {
                using (var uow = new InnerUnitOfWorkCompleteHandle())
                {

                }
            }).Message.ShouldBe(InnerUnitOfWorkCompleteHandle.DidNotCallCompleteMethodExceptionMessage);
        }

        /// <summary>
        /// 如果用户手动抛出异常，则该异常不会被覆盖。
        /// </summary>
        [Fact]
        public void Should_Not_Override_Exception_If_Exception_Is_Thrown_By_User()
        {
            Assert.Throws<Exception>(
                new Action(() =>
                {
                    using (var uow = new InnerUnitOfWorkCompleteHandle())
                    {
                        throw new Exception("内部异常!");
                    }
                })).Message.ShouldBe("内部异常!");
        }
    }
}
