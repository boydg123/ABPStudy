using Xunit;
using Shouldly;
using System.Linq;
using System.Reflection;
using Abp.Modules;
using Abp;
using Abp.Reflection;

namespace Abp.Test.Modules
{
    /// <summary>
    /// Abp 程序集查找器测试
    /// </summary>
    public class AbpAssemblyFinder_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 获取模块以及附加组件程序集
        /// </summary>
        [Fact]
        public void Should_Get_Module_And_Additional_Assemblies()
        {
            var bootstrapper = AbpBootstrapper.Create<MyStartupModule>(localIocManager);
            bootstrapper.Initialize();

            var assemblies = bootstrapper.IocManager.Resolve<AbpAssemblyFinder>().GetAllAssemblies();

            assemblies.Count.ShouldBe(3);

            assemblies.Any(a => a == typeof(MyStartupModule).Assembly).ShouldBeTrue();
            assemblies.Any(a => a == typeof(AbpKernelModule).Assembly).ShouldBeTrue();
            assemblies.Any(a => a == typeof(FactAttribute).Assembly).ShouldBeTrue();
        }

        /// <summary>
        /// 自定义启动模块
        /// </summary>
        public class MyStartupModule : AbpModule
        {
            /// <summary>
            /// 获取附加程序集
            /// </summary>
            /// <returns></returns>
            public override Assembly[] GetAdditionalAssemblies()
            {
                return new[] { typeof(FactAttribute).Assembly };
            }
        }
    }
}
