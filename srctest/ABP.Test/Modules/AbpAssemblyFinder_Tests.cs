using Xunit;
using Shouldly;
using System.Linq;
using System.Reflection;
using Abp.Modules;
using Abp;
using Abp.Reflection;

namespace ABP.Test.Modules
{
    public class AbpAssemblyFinder_Tests : TestBaseWithLocalManager
    {
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

        public class MyStartupModule : AbpModule
        {
            public override Assembly[] GetAdditionalAssemblies()
            {
                return new[] { typeof(FactAttribute).Assembly };
            }
        }
    }
}
