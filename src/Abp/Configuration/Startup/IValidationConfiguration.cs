using System;
using System.Collections.Generic;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 验证配置
    /// </summary>
    public interface IValidationConfiguration
    {
        /// <summary>
        /// 忽略的类型列表
        /// </summary>
        List<Type> IgnoredTypes { get; }
    }
}