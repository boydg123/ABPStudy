using Abp.Modules;
using Xunit;
using Shouldly;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Abp.Test.Modules
{
    /// <summary>
    /// 启动模块必须在最后
    /// </summary>
    [TestClass]
    public class StartupModuleToBeLast_Tests : TestBaseWithLocalManager
    {
        public class MyModule1 : AbpModule { }
        public class MyModule2 : AbpModule { }
        [DependsOn(typeof(MyModule1), typeof(MyModule2))]
        public class MyStartupModule : AbpModule { }
        public class MyPlugInDependedModule : AbpModule { }
        [DependsOn(typeof(MyPlugInDependedModule))]
        public class MyPlugInModule : AbpModule { }
        /// <summary>
        /// 启动模块必须是最后模块
        /// </summary>
        [TestMethod]
        public void StartupModule_ShouldBe_LastModule()
        {
            var bootstrapper = AbpBootstrapper.Create<MyStartupModule>(localIocManager);
            bootstrapper.Initialize();
            var modules = bootstrapper.IocManager.Resolve<IAbpModuleManager>().Modules;
            modules.Count.ShouldBe(4);

            modules.Any(m => m.Type == typeof(AbpKernelModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyStartupModule)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule1)).ShouldBeTrue();
            modules.Any(m => m.Type == typeof(MyModule2)).ShouldBeTrue();

            var startupModule = modules.Last();
            startupModule.Type.ShouldBe(typeof(MyStartupModule));
        }
    }
}
