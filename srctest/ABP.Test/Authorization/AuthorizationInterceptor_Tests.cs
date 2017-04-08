using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Authorization;

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

        }
    }
}
