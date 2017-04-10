using System;
using System.Collections.Generic;

namespace Abp.Web.Api.ProxyScripting.Configuration
{
    /// <summary>
    /// API代理脚本配置
    /// </summary>
    public class ApiProxyScriptingConfiguration : IApiProxyScriptingConfiguration
    {
        /// <summary>
        /// 生成器类型列表
        /// </summary>
        public IDictionary<string, Type> Generators { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiProxyScriptingConfiguration()
        {
            Generators = new Dictionary<string, Type>();
        }
    }
}