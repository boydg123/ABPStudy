using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;

namespace Abp.Web.SignalR
{
    /// <summary>
    /// Replaces <see cref="DefaultDependencyResolver"/> to resolve dependencies from Castle Windsor (<see cref="IWindsorContainer"/>).
    /// 从Castle Windsor (<see cref="IWindsorContainer"/>)来解析依赖替换<see cref="DefaultDependencyResolver"/>
    /// </summary>
    public class WindsorDependencyResolver : DefaultDependencyResolver
    {
        /// <summary>
        /// Windsor容器
        /// </summary>
        private readonly IWindsorContainer _windsorContainer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windsorContainer">Windsor 容器</param>
        public WindsorDependencyResolver(IWindsorContainer windsorContainer)
        {
            _windsorContainer = windsorContainer;
        }
        
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public override object GetService(Type serviceType)
        {
            return _windsorContainer.Kernel.HasComponent(serviceType)
                ? _windsorContainer.Resolve(serviceType)
                : base.GetService(serviceType);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _windsorContainer.Kernel.HasComponent(serviceType)
                ? _windsorContainer.ResolveAll(serviceType).Cast<object>()
                : base.GetServices(serviceType);
        }
    }
}