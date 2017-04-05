using Abp.Dependency;
using Xunit;
using Shouldly;
using NSubstitute;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using System.Transactions;

namespace ABP.Test.Domain.Uow
{
    public class UnitOfWorkManager_Tests : TestBaseWithLocalManager
    {
        [Fact]
        public void Should_Call_Uow_Methods()
        {
            var fakeUow = Substitute.For<IUnitOfWork>(); //伪造一个Uow

            localIocManager.IocContainer.Register(
                    Component.For<IUnitOfWorkDefaultOptions>().ImplementedBy<UnitOfWorkDefaultOptions>().LifestyleSingleton(),
                    Component.For<ICurrentUnitOfWorkProvider>().ImplementedBy<CallContextCurrentUnitOfWorkProvider>().LifestyleSingleton(),
                    Component.For<IUnitOfWorkManager>().ImplementedBy<UnitOfWorkManager>().LifestyleSingleton(),
                    Component.For<IUnitOfWork>().UsingFactoryMethod(() => fakeUow).LifestyleSingleton()
                );

            var uowManager = localIocManager.Resolve<IUnitOfWorkManager>();

            //开启第一个UOW
            using (var uow1 = uowManager.Begin())
            {
                //开始调用begin方法
                fakeUow.Received(1).Begin(Arg.Any<UnitOfWorkOptions>());
                //试图开始UOW(不启动一个新的，使用外部)
                using (var uow2 = uowManager.Begin())
                {
                    //自从有了当前UOW,Begin方法被调用
                    fakeUow.Received(1).Begin(Arg.Any<UnitOfWorkOptions>());

                    uow2.Complete();

                    //自从外部的UOW应该完成它后，Complete方法无效
                    fakeUow.DidNotReceive().Complete();
                }

                //试图开始UOW(强制开启一个新的)
                using (var uow2 = uowManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    //Begin方法被调用用来创建一个内部UOW
                    fakeUow.Received(2).Begin(Arg.Any<UnitOfWorkOptions>());

                    uow2.Complete();

                    //内部UOW应该被完成
                    fakeUow.Received(1).Complete();
                }

                //完成外部UOW
                uow1.Complete();
            }

            fakeUow.Received(2).Complete();
            fakeUow.Received(2).Dispose();
        }
    }
}
