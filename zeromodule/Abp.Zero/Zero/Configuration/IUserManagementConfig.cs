using Abp.Collections;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 用户管理配置
    /// </summary>
    public interface IUserManagementConfig
    {
        ITypeList<object> ExternalAuthenticationSources { get; set; }
    }
}