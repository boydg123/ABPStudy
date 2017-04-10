using System;
using System.Collections.Generic;

namespace Abp.Web.Api.ProxyScripting.Configuration
{
    /// <summary>
    /// API代理脚本配置
    /// </summary>
    public interface IApiProxyScriptingConfiguration
    {
        /// <summary>
        /// Used to add/replace proxy script generators. 
        /// 用户添加/替换代理脚本生成器
        /// </summary>
        IDictionary<string, Type> Generators { get; }
    }
}