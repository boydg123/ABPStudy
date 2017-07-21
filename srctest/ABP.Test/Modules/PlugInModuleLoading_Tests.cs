using Xunit;
using Shouldly;
using Abp.Modules;
using Abp.PlugIns;
using System.Linq;

namespace Abp.Test.Modules
{
    /// <summary>
    /// 插件模块加载测试
    /// </summary>
    public class PlugInModuleLoading_Tests : TestBaseWithLocalManager
    {
        public class MyModule1 : AbpModule
        { }
        public class MyModule2 : AbpModule
        { }

        [DependsOn(typeof(MyModule1),typeof(MyModule2))]
        public class MyStartupModule : AbpModule
        { }

        public class MyNotDependedModule : AbpModule
        { }

        public class MyPlugInDependedModule : AbpModule
        { }

        [DependsOn(typeof(MyPlugInDependedModule))]
        public class MyPlugInModule : AbpModule
        { }

        /// <summary>
        /// 加载所有Module
        /// </summary>
        [Fact]
        public void Should_Load_All_Modules()
        {
            var bootstrapper = AbpBootstrapper.Create<MyStartupModule>(localIocManager);
            bootstrapper.PlugInSources.AddTypeList(typeof(MyPlugInModule));
            bootstrapper.Initialize();

            var modules = bootstrapper.IocManager.Resolve<IAbpModuleManager>().Modules;

            modules.Count.ShouldBe(6);
            //不仅加载了相关依赖模块，也加载了插件模块以及插件依赖模块。
            modules.Any(m => m.Type == typeof(AbpKernelModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyStartupModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule1)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule2)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyPlugInDependedModule)).ShouldBeTrue();

            modules.Any(m => m.Type == typeof(MyNotDependedModule)).ShouldBeFalse();
        }
    }
}
