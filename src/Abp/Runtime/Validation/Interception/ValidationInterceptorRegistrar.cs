using Abp.Application.Services;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Abp.Runtime.Validation.Interception
{
    /// <summary>
    /// 注册验证拦截器
    /// </summary>
    internal static class ValidationInterceptorRegistrar
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        /// <summary>
        /// 核心组建注册
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="handler">处理器</param>
        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (typeof(IApplicationService).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(ValidationInterceptor)));
            }
        }
    }
}