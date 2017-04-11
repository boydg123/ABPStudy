using System.Collections.Generic;
using System.Web.Http;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace Abp.WebApi.Configuration
{
    /// <summary>
    /// Used to configure ABP WebApi module.
    /// 用于配置ABP WebApi模块
    /// </summary>
    public interface IAbpWebApiConfiguration
    {
        /// <summary>
        /// Default UnitOfWorkAttribute for all actions.
        /// 所有Action的默认工作单元标记
        /// </summary>
        UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        /// <summary>
        /// Default WrapResultAttribute for all actions.
        /// 所有Action的默认Result包装标记
        /// </summary>
        WrapResultAttribute DefaultWrapResultAttribute { get; }

        /// <summary>
        /// Default WrapResultAttribute for all dynamic web api actions.
        /// 为所有动态web api action的默认Result包装标记
        /// </summary>
        WrapResultAttribute DefaultDynamicApiWrapResultAttribute { get; }

        /// <summary>
        /// List of URLs to ignore on result wrapping.
        /// 忽略包装结果的Url列表
        /// </summary>
        List<string> ResultWrappingIgnoreUrls { get; }

        /// <summary>
        /// Gets/sets <see cref="HttpConfiguration"/>.
        /// 获取/设置 <see cref="HttpConfiguration"/>
        /// </summary>
        HttpConfiguration HttpConfiguration { get; set; }

        /// <summary>
        /// Default: true.
        /// 所有控制器是否验证可用。默认：true
        /// </summary>
        bool IsValidationEnabledForControllers { get; set; }

        /// <summary>
        /// Default: true.
        /// 是否自动防伪验证可用。默认：true
        /// </summary>
        bool IsAutomaticAntiForgeryValidationEnabled { get; set; }

        /// <summary>
        /// Default: true.
        /// 为所有的响应不设置Cache。默认：true
        /// </summary>
        bool SetNoCacheForAllResponses { get; set; }

        /// <summary>
        /// Used to configure dynamic Web API controllers.
        /// 用于配置动态Web API控制器
        /// </summary>
        IDynamicApiControllerBuilder DynamicApiControllerBuilder { get; }
    }
}