using System;
using System.Collections.Generic;

namespace Abp.Auditing
{
    /// <summary>
    /// Used to configure auditing.
    /// 用于审计配置
    /// </summary>
    public interface IAuditingConfiguration
    {
        /// <summary>
        /// Used to enable/disable auditing system.Default: true. Set false to completely disable it.
        /// 用于启用或禁用审计系统,默认为：true，设置为false将完全地禁用它
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Set true to enable saving audit logs if current user is not logged in.Default: false.
        /// 设置为true，启用保存未登录用户的保存操作的审计日志,默认为: false.
        /// </summary>
        bool IsEnabledForAnonymousUsers { get; set; }

        /// <summary>
        /// List of selectors to select classes/interfaces which should be audited as default.
        /// 获取将被审计的类/接口的selector列表
        /// </summary>
        IAuditingSelectorList Selectors { get; }

        /// <summary>
        /// Ignored types for serialization on audit logging.
        /// 忽略审计日志记录的序列化类型
        /// </summary>
        List<Type> IgnoredTypes { get; }
    }
}