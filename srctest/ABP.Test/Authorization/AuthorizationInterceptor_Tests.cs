using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Authorization;
using Castle.MicroKernel.Registration;
using Abp.Application.Features;
using NSubstitute;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace ABP.Test.Authorization
{
    /// <summary>
    /// 授权拦截器测试
    /// </summary>
    public class AuthorizationInterceptor_Tests : TestBaseWithLocalManager
    {
        #region 授权类
        /// <summary>
        /// 授权 - 同步
        /// </summary>
        public class MyClassToBeAuthorized_Sync
        {
            /// <summary>
            /// 不需要权限即可调用的方法
            /// </summary>
            public bool Called_MethodWithOutPermission { get; private set; }
            /// <summary>
            /// 需要权限1才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1 { get; private set; }
            /// <summary>
            /// 需要权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission3 { get; private set; }
            /// <summary>
            /// 需要权限1或权限2才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission2 { get; private set; }
            /// <summary>
            /// 需要权限1或权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission3 { get; private set; }
            /// <summary>
            /// 需要权限1和权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission3WithRequireAll { get; private set; }
            public virtual void MethodWithoutPermission()
            {
                Called_MethodWithOutPermission = true;
            }
            [AbpAuthorize("Permission1")]
            public virtual int MethodWithPermission1()
            {
                Called_MethodWithPermission1 = true;
                return 42;
            }

            [AbpAuthorize("Permission3")]
            public virtual void MethodWithPermission3()
            {
                Called_MethodWithPermission3 = true;
            }

            [AbpAuthorize("Permission1", "Permission2")]
            public virtual void MethodWithPermission1AndPermission2()
            {
                Called_MethodWithPermission1AndPermission2 = true;
            }

            [AbpAuthorize("Permission1", "Permission3")]
            public virtual void MethodWithPermission1AndPermission3()
            {
                Called_MethodWithPermission1AndPermission3 = true;
            }

            [AbpAuthorize("Permission1", "Permission3", RequireAllPermissions = true)]
            public virtual void MethodWithPermission1AndPermission3WithRequireAll()
            {
                Called_MethodWithPermission1AndPermission3WithRequireAll = true;
            }
        }

        /// <summary>
        /// 授权 - 异步
        /// </summary>
        public class MyClassToBeAuthorized_Async
        {
            /// <summary>
            /// 不需要权限即可调用的方法
            /// </summary>
            public bool Called_MethodWithoutPermission { get; private set; }
            /// <summary>
            /// 需要权限1才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1 { get; private set; }
            /// <summary>
            /// 需要权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission3 { get; private set; }
            /// <summary>
            /// 需要权限1或权限2才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission2 { get; private set; }
            /// <summary>
            /// 需要权限1或权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission3 { get; private set; }
            /// <summary>
            /// 需要权限1和权限3才能调用的方法
            /// </summary>
            public bool Called_MethodWithPermission1AndPermission3WithRequireAll { get; private set; }

            public virtual async Task MethodWithoutPermission()
            {
                Called_MethodWithoutPermission = true;
                await Task.Delay(1);
            }

            [AbpAuthorize("Permission1")]
            public virtual async Task<int> MethodWithPermission1Async()
            {
                Called_MethodWithPermission1 = true;
                await Task.Delay(1);
                return 42;
            }

            [AbpAuthorize("Permission3")]
            public virtual async Task MethodWithPermission3Async()
            {
                Called_MethodWithPermission3 = true;
                await Task.Delay(1);
            }

            [AbpAuthorize("Permission1", "Permission2")]
            public virtual async Task MethodWithPermission1AndPermission2Async()
            {
                Called_MethodWithPermission1AndPermission2 = true;
                await Task.Delay(1);
            }

            [AbpAuthorize("Permission1", "Permission3")]
            public virtual async Task MethodWithPermission1AndPermission3Async()
            {
                Called_MethodWithPermission1AndPermission3 = true;
                await Task.Delay(1);
            }

            [AbpAuthorize("Permission1", "Permission3", RequireAllPermissions = true)]
            public virtual async Task MethodWithPermission1AndPermission3WithRequireAllAsync()
            {
                Called_MethodWithPermission1AndPermission3WithRequireAll = true;
                await Task.Delay(1);
            }
        }
        #endregion

        private readonly MyClassToBeAuthorized_Async _asyncObj;
        private readonly MyClassToBeAuthorized_Sync _syncObj;

        /// <summary>
        /// 构造函数 - 初始化一堆东西
        /// </summary>
        public AuthorizationInterceptor_Tests()
        {
            //模拟一个IFeatureChecker
            localIocManager.IocContainer.Register(Component.For<IFeatureChecker>().Instance(Substitute.For<IFeatureChecker>()));
            localIocManager.IocContainer.Register(Component.For<IAuthorizationConfiguration, AuthorizationConfiguration>());
            localIocManager.Register<AuthorizationInterceptor>(DependencyLifeStyle.Transient);
            localIocManager.Register<IAuthorizationHelper, AuthorizationHelper>(DependencyLifeStyle.Transient);
            //给两个验证类注册验证拦截器
            localIocManager.IocContainer.Register(
                Component.For<MyClassToBeAuthorized_Sync>().Interceptors<AuthorizationInterceptor>().LifestyleTransient(),
                Component.For<MyClassToBeAuthorized_Async>().Interceptors<AuthorizationInterceptor>().LifestyleTransient()
            );
            //模拟Session
            var session = Substitute.For<IAbpSession>();
            session.TenantId.Returns(1);
            session.UserId.Returns(1);
            //如果从IOC获取IAbpSession，则将模拟的session给它赋值
            //如果没有模拟Session，则会抛出异常用户没有登录
            localIocManager.IocContainer.Register(Component.For<IAbpSession>().UsingFactoryMethod(() => session));

            //模拟权限检查器
            var permissionChecker = Substitute.For<IPermissionChecker>();
            permissionChecker.IsGrantedAsync("Permission1").Returns(true);//权限1 永远返回True
            permissionChecker.IsGrantedAsync("Permission2").Returns(true);
            permissionChecker.IsGrantedAsync("Permission3").Returns(false);
            //如果从IOC容器中获取IPermissionChecker,则返回的对象为permissionChceker
            localIocManager.IocContainer.Register(Component.For<IPermissionChecker>().UsingFactoryMethod(() => permissionChecker));

            _syncObj = localIocManager.Resolve<MyClassToBeAuthorized_Sync>();
            _asyncObj = localIocManager.Resolve<MyClassToBeAuthorized_Async>();
        }

        /// <summary>
        /// 测试授权 - 同步
        /// </summary>
        [Fact]
        public void Test_Authorization_Sync()
        {
            //验证授权的方法
            _syncObj.MethodWithoutPermission();
            _syncObj.Called_MethodWithOutPermission.ShouldBe(true);

            _syncObj.MethodWithPermission1().ShouldBe(42);
            _syncObj.Called_MethodWithPermission1.ShouldBe(true);

            _syncObj.MethodWithPermission1AndPermission2();
            _syncObj.Called_MethodWithPermission1AndPermission2.ShouldBe(true);

            _syncObj.MethodWithPermission1AndPermission3();
            _syncObj.Called_MethodWithPermission1AndPermission3.ShouldBe(true);

            //验证没授权方法
            Assert.Throws<AbpAuthorizationException>(() => _syncObj.MethodWithPermission3());
            _syncObj.Called_MethodWithPermission3.ShouldBe(false);

            Assert.Throws<AbpAuthorizationException>(() => _syncObj.MethodWithPermission1AndPermission3WithRequireAll());
            _syncObj.Called_MethodWithPermission1AndPermission3WithRequireAll.ShouldBe(false);
        }

        /// <summary>
        /// 授权测试 - 异步
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_Authorization_Async()
        {
            //验证授权方法
            await _asyncObj.MethodWithoutPermission();
            _asyncObj.Called_MethodWithoutPermission.ShouldBe(true);

            (await _asyncObj.MethodWithPermission1Async()).ShouldBe(42);
            _asyncObj.Called_MethodWithPermission1.ShouldBe(true);

            await _asyncObj.MethodWithPermission1AndPermission2Async();
            _asyncObj.Called_MethodWithPermission1AndPermission2.ShouldBe(true);

            await _asyncObj.MethodWithPermission1AndPermission3Async();
            _asyncObj.Called_MethodWithPermission1AndPermission3.ShouldBe(true);

            //验证没有授权方法

            await Assert.ThrowsAsync<AbpAuthorizationException>(async () => await _asyncObj.MethodWithPermission3Async());
            _asyncObj.Called_MethodWithPermission3.ShouldBe(false);

            await Assert.ThrowsAsync<AbpAuthorizationException>(async () => await _asyncObj.MethodWithPermission1AndPermission3WithRequireAllAsync());
            _asyncObj.Called_MethodWithPermission1AndPermission3WithRequireAll.ShouldBe(false);
        }
    }
}