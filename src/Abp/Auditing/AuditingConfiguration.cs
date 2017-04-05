using System;
using System.Collections.Generic;

namespace Abp.Auditing
{
    /// <summary>
    /// 审计配置
    /// </summary>
    internal class AuditingConfiguration : IAuditingConfiguration
    {
        /// <summary>
        /// 用于启用或禁用审计系统,默认为：true，设置为false将完全地禁用它
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 设置为true，启用保存未匿名用户的保存操作的审计日志,默认为: false.
        /// </summary>
        public bool IsEnabledForAnonymousUsers { get; set; }

        /// <summary>
        /// 将被审计的获取类/接口的selector列表
        /// </summary>
        public IAuditingSelectorList Selectors { get; }

        /// <summary>
        /// 忽略的类型列表
        /// </summary>
        public List<Type> IgnoredTypes { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuditingConfiguration()
        {
            IsEnabled = true;
            Selectors = new AuditingSelectorList();
            IgnoredTypes = new List<Type>();
        }
    }
}