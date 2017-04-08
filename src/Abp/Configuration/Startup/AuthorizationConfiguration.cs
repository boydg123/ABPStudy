using Abp.Authorization;
using Abp.Collections;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 用于配置授权
    /// </summary>
    public class AuthorizationConfiguration : IAuthorizationConfiguration
    {
        /// <summary>
        /// 授权提供者
        /// </summary>
        public ITypeList<AuthorizationProvider> Providers { get; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorizationConfiguration()
        {
            Providers = new TypeList<AuthorizationProvider>();
            IsEnabled = true;
        }
    }
}