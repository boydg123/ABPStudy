using System.Collections.Generic;
using System.Web.Http;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace Abp.WebApi.Configuration
{
    /// <summary>
    /// 用于配置ABP WebApi模块
    /// </summary>
    internal class AbpWebApiConfiguration : IAbpWebApiConfiguration
    {
        /// <summary>
        /// 所有Action的默认工作单元标记
        /// </summary>
        public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        /// <summary>
        /// 所有Action的默认Result包装标记
        /// </summary>
        public WrapResultAttribute DefaultWrapResultAttribute { get; }

        /// <summary>
        /// 为所有动态web api action的默认Result包装标记
        /// </summary>
        public WrapResultAttribute DefaultDynamicApiWrapResultAttribute { get; }

        /// <summary>
        /// 忽略包装结果的Url列表
        /// </summary>
        public List<string> ResultWrappingIgnoreUrls { get; }

        /// <summary>
        /// 获取/设置 <see cref="HttpConfiguration"/>
        /// </summary>
        public HttpConfiguration HttpConfiguration { get; set; }

        /// <summary>
        /// 所有控制器是否验证可用。默认：true
        /// </summary>
        public bool IsValidationEnabledForControllers { get; set; }

        /// <summary>
        /// 是否自动防伪验证可用。默认：true
        /// </summary>
        public bool IsAutomaticAntiForgeryValidationEnabled { get; set; }

        /// <summary>
        /// 为所有的响应不设置Cache。默认：true
        /// </summary>
        public bool SetNoCacheForAllResponses { get; set; }

        /// <summary>
        /// 用于配置动态Web API控制器
        /// </summary>
        public IDynamicApiControllerBuilder DynamicApiControllerBuilder { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dynamicApiControllerBuilder">动态API控制器生成器</param>
        public AbpWebApiConfiguration(IDynamicApiControllerBuilder dynamicApiControllerBuilder)
        {
            DynamicApiControllerBuilder = dynamicApiControllerBuilder;

            HttpConfiguration = GlobalConfiguration.Configuration;
            DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
            DefaultWrapResultAttribute = new WrapResultAttribute(false);
            DefaultDynamicApiWrapResultAttribute = new WrapResultAttribute();
            ResultWrappingIgnoreUrls = new List<string>();
            IsValidationEnabledForControllers = true;
            IsAutomaticAntiForgeryValidationEnabled = true;
            SetNoCacheForAllResponses = true;
        }
    }
}