using System;
using System.Collections.Generic;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 验证配置
    /// </summary>
    public class ValidationConfiguration : IValidationConfiguration
    {
        /// <summary>
        /// 忽略的类型列表
        /// </summary>
        public List<Type> IgnoredTypes { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidationConfiguration()
        {
            IgnoredTypes = new List<Type>();
        }
    }
}