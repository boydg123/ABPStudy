using Abp.Authorization;
using Xunit;
using Shouldly;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Configuration.Startup;

namespace ABP.Test.Authorization
{
    /// <summary>
    /// 授权帮助类测试
    /// </summary>
    public class AuthorizationHelper_Tests
    {
        private readonly AuthorizationHelper _authorizationHelper;

        /// <summary>
        /// 构造函数，初始化授权帮助类。模拟一些对象
        /// </summary>
        public AuthorizationHelper_Tests()
        {
            //模拟一个功能检查器
            var featureChecker = Substitute.For<IFeatureChecker>();
            featureChecker.GetValueAsync(Arg.Any<string>()).Returns("false"); //一直返回false
            //模拟一个权限检查器
            var permissionChecker = Substitute.For<IPermissionChecker>();
            permissionChecker.IsGrantedAsync(Arg.Any<string>()).Returns(false); //一直返回false

            var configuration = Substitute.For<IAuthorizationConfiguration>();
            configuration.IsEnabled.Returns(true); //启用基于身份验证和授权

            _authorizationHelper = new AuthorizationHelper(featureChecker, configuration)
            {
                PermissionChecker = permissionChecker
            };
        }

        /// <summary>
        /// 授权类调用匿名方法。没有授权类，可以调用普通方法
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task NotAuthorizedMethodsCanBeCalledAnonymously()
        {
            //没有加授权标记的方法可以被调用
            await _authorizationHelper.AuthorizeAsync(typeof(MyNonAuthorizedClass).GetMethod(nameof(MyNonAuthorizedClass.Test_NotAuthorized)));
            //匿名方法在授权类中可以被调用
            await _authorizationHelper.AuthorizeAsync(typeof(MyAuthorizedClass).GetMethod(nameof(MyAuthorizedClass.Test_NotAuthorized)));
        }

        /// <summary>
        /// 在没有授权类中调用授权方法会发生异常。
        /// 在授权类中调用普通方法会发生异常
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AuthorizedMethodsCanNotBeCalledAnonymously()
        {
            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await _authorizationHelper.AuthorizeAsync(
                    typeof(MyNonAuthorizedClass).GetMethod(nameof(MyNonAuthorizedClass.Test_Authorized))
                );
            });

            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await _authorizationHelper.AuthorizeAsync(
                    typeof(MyAuthorizedClass).GetMethod(nameof(MyAuthorizedClass.Test_Authorized))
                );
            });
        }
        public class MyNonAuthorizedClass
        {
            public void Test_NotAuthorized()
            {
            }

            /// <summary>
            /// 授权用户才能访问
            /// </summary>
            [AbpAuthorize]
            public void Test_Authorized()
            { }
        }

        /// <summary>
        /// 授权用户才能访问
        /// </summary>
        [AbpAuthorize]
        public class MyAuthorizedClass
        {
            /// <summary>
            /// 匿名方法
            /// </summary>
            [AbpAllowAnonymous]
            public void Test_NotAuthorized()
            { }

            public void Test_Authorized()
            { }
        }
    }
}
