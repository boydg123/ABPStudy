using Abp.Authorization;
using Abp.Collections;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure authorization system.
    /// 用于配置授权
    /// </summary>
    public interface IAuthorizationConfiguration
    {
        /// <summary>
        /// List of authorization providers.
        /// 授权提供者列表
        /// </summary>
        ITypeList<AuthorizationProvider> Providers { get; }

        /// <summary>
        /// Enables/Disables attribute based authentication and authorization.
        /// 启用/禁用属性基于身份验证和授权
        /// Default: true.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}