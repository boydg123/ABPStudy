using System;
using System.Linq;
using System.Reflection;
using Abp.Application.Features;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Abp.Authorization
{
    /// <summary>
    /// This class is used to register interceptors on the Application Layer.
    /// 此类用于在应用层注册拦截器
    /// </summary>
    internal static class AuthorizationInterceptorRegistrar
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        public static void Initialize(IIocManager iocManager)
        {
            //组件注册事件
            iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;            
        }

        /// <summary>
        /// 组件注册事件处理器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (ShouldIntercept(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuthorizationInterceptor))); 
            }
        }

        /// <summary>
        /// 应该拦截
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool ShouldIntercept(Type type)
        {
            if (SelfOrMethodsDefinesAttribute<AbpAuthorizeAttribute>(type))
            {
                return true;
            }

            if (SelfOrMethodsDefinesAttribute<RequiresFeatureAttribute>(type))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 自己或方法定义特性
        /// </summary>
        /// <typeparam name="TAttr">特性对象</typeparam>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool SelfOrMethodsDefinesAttribute<TAttr>(Type type)
        {
            if (type.IsDefined(typeof(TAttr), true))
            {
                return true;
            }

            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(m => m.IsDefined(typeof(TAttr), true));
        }
    }
}