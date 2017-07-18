using System;
using Abp.Dependency;
using Xunit;
using Shouldly;
using System.Collections.Generic;

namespace Abp.Test.Dependency
{
    /// <summary>
    /// IOC作用域解析器测试
    /// </summary>
    public class ScopedIocResolver_Tests : TestBaseWithLocalManager
    {
        #region 测试方法
        /// <summary>
        /// 使用UsingScope解析对象。对象只在作用域内起作用，作用域外则就自动释放了
        /// </summary>
        [Fact]
        public void UsingScope_Test_ShouldWork()
        {
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);

            SimpleDisposableObject simpleObj = null;

            //scope为Action中的参数IScopedIocResolver
            localIocManager.UsingScope(scope =>
            {
                simpleObj = scope.Resolve<SimpleDisposableObject>();
            });

            simpleObj.DisposeCount.ShouldBe(1);
        }

        /// <summary>
        /// 使用UsingScope解析对象,并且使用构造函数初始化对象
        /// </summary>
        [Fact]
        public void UsingScope_Test_With_Constructor_ShouldWork()
        {
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);

            SimpleDisposableObject simpleObj = null;

            localIocManager.UsingScope(scope =>
            {
                //使用构造函数传递参数，然后解析对象
                simpleObj = scope.Resolve<SimpleDisposableObject>(new
                {
                    myData = 40
                });
            });
            simpleObj.MyData.ShouldBe(40);
        }

        /// <summary>
        /// IOC作用域解析器解析对象
        /// </summary>
        [Fact]
        public void IIocScopeResolve_Test_ShouldWork()
        {
            //注册对象
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject2>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject3>(DependencyLifeStyle.Transient);

            SimpleDisposableObject simpleObj = null;
            SimpleDisposableObject2 simpleObj2 = null;
            SimpleDisposableObject3 simpleObj3 = null;

            //在一个作用域内使用解析
            using (var scope = localIocManager.CreateScope())
            {
                simpleObj = scope.Resolve<SimpleDisposableObject>();
                simpleObj2 = scope.Resolve<SimpleDisposableObject2>();
                simpleObj3 = scope.Resolve<SimpleDisposableObject3>();
            }

            simpleObj.DisposeCount.ShouldBe(1);
            simpleObj2.DisposeCount.ShouldBe(1);
            simpleObj3.DisposeCount.ShouldBe(1);
        }

        /// <summary>
        /// IOC作用域解析器解析对象,并用构造函数初始化对象
        /// </summary>
        [Fact]
        public void IIocScopeResolve_Test_With_Constructor_ShouldWork()
        {
            //注册对象
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject2>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject3>(DependencyLifeStyle.Transient);

            SimpleDisposableObject simpleObj = null;
            SimpleDisposableObject2 simpleObj2 = null;
            SimpleDisposableObject3 simpleObj3 = null;

            //在一个作用域内使用解析
            using (var scope = localIocManager.CreateScope())
            {
                simpleObj = scope.Resolve<SimpleDisposableObject>(new { myData = 40 });
                simpleObj2 = scope.Resolve<SimpleDisposableObject2>(new { myData = 41 });
                simpleObj3 = scope.Resolve<SimpleDisposableObject3>(new { myData = 42 });
            }

            simpleObj.MyData.ShouldBe(40);
            simpleObj2.MyData.ShouldBe(41);
            simpleObj3.MyData.ShouldBe(42);
        }

        /// <summary>
        /// 使用IOC作用域解析器，一次解析所有对象，作用域外，所有对象均被释放。
        /// </summary>
        [Fact]
        public void IIocScopedResolver_Test_ResolveAll_Should_DisposeAll_Registrants()
        {
            //注册对象
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency2>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency3>(DependencyLifeStyle.Transient);

            IEnumerable<ISimpleDependency> simpleDependencies;

            using (var scope = localIocManager.CreateScope())
            {
                simpleDependencies = scope.ResolveAll<ISimpleDependency>();
            }

            simpleDependencies.ShouldAllBe(simple => simple.DisposeCount == 1);

            foreach (var simple in simpleDependencies)
            {
                simple.DisposeCount.ShouldBe(1);
            }
        }

        /// <summary>
        /// 使用IOC作用域解析器，一次解析所有对象，作用域外，所有对象均被释放。并且通过构造函数初始化对象
        /// </summary>
        [Fact]
        public void IIocScopedResolver_Test_ResolveAll_Should_Work_With_Constructor()
        {
            //注册对象
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency2>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency3>(DependencyLifeStyle.Transient);

            IEnumerable<ISimpleDependency> simpleDependencies;

            using (var scope = localIocManager.CreateScope())
            {
                simpleDependencies = scope.ResolveAll<ISimpleDependency>(new { myData = 40 });
            }

            simpleDependencies.ShouldAllBe(simple => simple.MyData == 40 && simple.DisposeCount == 1);

            foreach (var simple in simpleDependencies)
            {
                simple.DisposeCount.ShouldBe(1);
                simple.MyData.ShouldBe(40);
            }
        }

        /// <summary>
        /// IOC作用域解析器解析所有和其他对象一起解析的时候应该工作。
        /// </summary>
        [Fact]
        public void IIocScopedResolver_Test_ResolveAll_Should_Work_With_OtherResolvings()
        {
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency2>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency3>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);

            IEnumerable<ISimpleDependency> simpleDependendcies;
            SimpleDisposableObject simpleObject;

            using (var scope = localIocManager.CreateScope())
            {
                simpleDependendcies = scope.ResolveAll<ISimpleDependency>();
                simpleObject = scope.Resolve<SimpleDisposableObject>();
            }

            simpleDependendcies.ShouldAllBe(x => x.DisposeCount == 1);
            simpleObject.DisposeCount.ShouldBe(1);
        }

        /// <summary>
        /// IOC作用域解析器解析所有和其他对象一起解析的时候应该工作。并且使用构造函数初始化对象
        /// </summary>
        [Fact]
        public void IIocScopedResolver_Test_ResolveAll_Should_Work_With_OtherResolvings_ConstructorArguments()
        {
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency2>(DependencyLifeStyle.Transient);
            localIocManager.Register<ISimpleDependency, SimpleDependency3>(DependencyLifeStyle.Transient);
            localIocManager.Register<SimpleDisposableObject>(DependencyLifeStyle.Transient);

            IEnumerable<ISimpleDependency> simpleDependendcies;
            SimpleDisposableObject simpleObject;

            using (var scope = localIocManager.CreateScope())
            {
                simpleDependendcies = scope.ResolveAll<ISimpleDependency>(new { myData = 40 });
                simpleObject = scope.Resolve<SimpleDisposableObject>(new { myData = 40 });
            }

            simpleDependendcies.ShouldAllBe(x => x.MyData == 40);
            simpleObject.MyData.ShouldBe(40);
        }

        /// <summary>
        /// IOC作用域解析器，是否注册方法测试
        /// </summary>
        [Fact]
        public void IIocScopeResolver_Test_IsRegistered_ShouldWork()
        {
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);
            using (var scope = localIocManager.CreateScope())
            {
                scope.IsRegistered<ISimpleDependency>().ShouldBe(true);
                scope.IsRegistered(typeof(ISimpleDependency)).ShouldBe(true);
                scope.IsRegistered<SimpleDependency>().ShouldBe(true);
                scope.IsRegistered(typeof(SimpleDependency)).ShouldBe(true);
            }
        }

        /// <summary>
        /// IOC作用域解析器，自己释放对象
        /// </summary>
        [Fact]
        public void IIocScopedResolver_Test_Custom_Release_ShouldWork()
        {
            localIocManager.Register<ISimpleDependency, SimpleDependency>(DependencyLifeStyle.Transient);

            ISimpleDependency simpleDependency;

            using (var scope = localIocManager.CreateScope())
            {
                simpleDependency = scope.Resolve<ISimpleDependency>();
                scope.Release(simpleDependency);
            }

            simpleDependency.DisposeCount.ShouldBe(1);
        }

        #endregion

        #region 接口与实现
        /// <summary>
        /// 简单接口
        /// </summary>
        public interface ISimpleDependency : IDisposable
        {
            int MyData { get; set; }
            int DisposeCount { get; set; }
        }

        /// <summary>
        /// 简单实现1
        /// </summary>
        public class SimpleDependency : ISimpleDependency
        {
            public int MyData { get; set; }

            public int DisposeCount { get; set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }

        /// <summary>
        /// 简单实现2
        /// </summary>
        public class SimpleDependency2 : ISimpleDependency
        {
            public int DisposeCount { get; set; }

            public int MyData { get; set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }

        /// <summary>
        /// 简单实现3
        /// </summary>
        public class SimpleDependency3 : ISimpleDependency
        {
            public int MyData { get; set; }

            public int DisposeCount { get; set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }
        #endregion
    }
}
