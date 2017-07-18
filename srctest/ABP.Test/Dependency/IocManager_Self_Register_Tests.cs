using Shouldly;
using Xunit;
using Abp.Dependency;

namespace Abp.Test.Dependency
{
    public class IocManager_Self_Register_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 自注册以及从所有的接口注册测试
        /// </summary>
        [Fact]
        public void Self_Register_With_All_Interfaces_Tests()
        {
            var registrar = localIocManager.Resolve<IIocRegistrar>();
            var resolver = localIocManager.Resolve<IIocResolver>();
            var managerByInterface = localIocManager.Resolve<IIocManager>();
            var managerByClass = localIocManager.Resolve<IocManager>();

            managerByClass.ShouldBeSameAs(registrar);
            managerByClass.ShouldBeSameAs(resolver);
            managerByClass.ShouldBeSameAs(managerByInterface);
        }
    }
}
