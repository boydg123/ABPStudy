using Abp.Collections;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 用户管理配置
    /// </summary>
    public class UserManagementConfig : IUserManagementConfig
    {
        /// <summary>
        /// 外部认证源列表
        /// </summary>
        public ITypeList<object> ExternalAuthenticationSources { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserManagementConfig()
        {
            ExternalAuthenticationSources = new TypeList();
        }
    }
}