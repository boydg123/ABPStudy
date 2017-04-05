using System;
using System.Linq;
using Abp.Dependency;
using Castle.Core;

namespace Abp.Auditing
{
    /// <summary>
    /// 审计拦截注册器
    /// </summary>
    internal static class AuditingInterceptorRegistrar
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        public static void Initialize(IIocManager iocManager)
        {
            var auditingConfiguration = iocManager.Resolve<IAuditingConfiguration>();
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (ShouldIntercept(auditingConfiguration, handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuditingInterceptor)));
                }
            };
        }

        /// <summary>
        /// 是否需要拦截
        /// </summary>
        /// <param name="auditingConfiguration">审计配置</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool ShouldIntercept(IAuditingConfiguration auditingConfiguration, Type type)
        {
            if (auditingConfiguration.Selectors.Any(selector => selector.Predicate(type)))
            {
                return true;
            }

            if (type.IsDefined(typeof(AuditedAttribute), true))
            {
                return true;
            }

            if (type.GetMethods().Any(m => m.IsDefined(typeof(AuditedAttribute), true)))
            {
                return true;
            }

            return false;
        }
    }
}