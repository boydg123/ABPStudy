using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Abp.Dependency;

namespace Abp.WebApi.Controllers
{
    /// <summary>
    /// This class is used to use IOC system to create api controllers.It's used by ASP.NET system.
    /// 此类用于使用IOC系统创建API控制器。它通过ASP.NET系统使用
    /// </summary>
    public class AbpApiControllerActivator : IHttpControllerActivator
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver"></param>
        public AbpApiControllerActivator(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <summary>
        /// 创建Http控制器
        /// </summary>
        /// <param name="request">Http请求消息</param>
        /// <param name="controllerDescriptor">Http控制器描述器</param>
        /// <param name="controllerType">控制器类型</param>
        /// <returns></returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controllerWrapper = _iocResolver.ResolveAsDisposable<IHttpController>(controllerType);
            request.RegisterForDispose(controllerWrapper);
            return controllerWrapper.Object;
        }
    }
}