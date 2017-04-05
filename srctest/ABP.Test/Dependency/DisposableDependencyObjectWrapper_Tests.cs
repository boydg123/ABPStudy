using Abp.Dependency;
using Xunit;
using Shouldly;

namespace ABP.Test.Dependency
{
    /// <summary>
    /// 一次性依赖对象包装器
    /// </summary>
    public class DisposableDependencyObjectWrapper_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 包装器解析
        /// </summary>
        [Fact]
        public void ResolveAsDisposable_Should_Work()
        {
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);
            SimpleDisposableObject simpleObj;
            using (var wrapper = localIocManager.ResolveAsDisposable<SimpleDisposableObject>())
            {
                wrapper.Object.ShouldNotBe(null);
                simpleObj = wrapper.Object;
            }
            simpleObj.DisposeCount.ShouldBe(1);
        }

        [Fact]
        public void ResolveAsDisposable_With_Constructor_Args_Should_Work()
        {
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);

            using (var wrapper = localIocManager.ResolveAsDisposable<SimpleDisposableObject>(new { myData = 42 }))
            {
                wrapper.Object.MyData.ShouldBe(42);
            }
        }

        [Fact]
        public void Using_Test()
        {
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);
            localIocManager.Using<SimpleDisposableObject>(
                obj => obj.MyData.ShouldBe(0)
                );
        }
    }
}
