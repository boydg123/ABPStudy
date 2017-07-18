using Shouldly;
using Xunit;
using System.Linq;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// IOC管理器测试
    /// </summary>
    public class IocManager_Tests : TestBaseWithLocalManager
    {
        public IocManager_Tests()
        {
            localIocManager.Register<IEmpty, EmptyImplOne>();
            localIocManager.Register<IEmpty, EmptyImplTwo>();
        }

        /// <summary>
        /// 如果一个接口有多个类的实现，应该获取注册的第一个类
        /// </summary>
        [Fact]
        public void Should_Get_First_Registered_Class_If_Registered_Multiple_Class_For_Same_Interface_Test()
        {
            localIocManager.Resolve<IEmpty>().GetType().ShouldBe(typeof(EmptyImplOne));
            localIocManager.Resolve<IEmpty>().GetType().ShouldNotBe(typeof(EmptyImplTwo));
        }

        /// <summary>
        /// 解析所有实现
        /// </summary>
        [Fact]
        public void ResolveAllImpl_Test()
        {
            var instances = localIocManager.ResolveAll<IEmpty>();
            instances.Length.ShouldBe(2);
            instances.Any(i => i.GetType() == typeof(EmptyImplOne)).ShouldBe(true);
            instances.Any(i => i.GetType() == typeof(EmptyImplTwo)).ShouldBe(true);
        }

        /// <summary>
        /// 空接口
        /// </summary>
        public interface IEmpty
        {

        }

        /// <summary>
        /// 接口实现1
        /// </summary>
        public class EmptyImplOne : IEmpty
        {

        }

        /// <summary>
        /// 接口实现2
        /// </summary>
        public class EmptyImplTwo : IEmpty
        {

        }
    }
}
