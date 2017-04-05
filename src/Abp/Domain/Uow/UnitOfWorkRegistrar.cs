using System.Linq;
using System.Reflection;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// This class is used to register interceptor for needed classes for Unit Of Work mechanism.
    /// 此类为需要使用工作单元的类注册拦截器
    /// </summary>
    internal static class UnitOfWorkRegistrar
    {
        /// <summary>
        /// Initializes the registerer.
        /// 初始化注册器
        /// </summary>
        /// <param name="iocManager">IOC manager / IOC管理器</param>
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += ComponentRegistered;
        }

        /// <summary>
        /// IOC组件注册处理函数
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="handler">处理器</param>
        private static void ComponentRegistered(string key, IHandler handler)
        {
            if (UnitOfWorkHelper.IsConventionalUowClass(handler.ComponentModel.Implementation))
            {
                //Intercept all methods of all repositories. / 拦截仓储的所有方法
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
            }
            else if (handler.ComponentModel.Implementation.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(UnitOfWorkHelper.HasUnitOfWorkAttribute))
            {
                //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
                //拦截至少有一个方法使用了UnitOfWork特性的类的所有方法
                //TODO: Intecept only UnitOfWork methods, not other methods!
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
            }
        }
    }
}