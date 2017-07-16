using Derrick.Security;

namespace Derrick.Authorization.Users.Profile.Dto
{
    /// <summary>
    /// 获取密码复杂度设置Output
    /// </summary>
    public class GetPasswordComplexitySettingOutput
    {
        /// <summary>
        /// 密码复杂度设置
        /// </summary>
        public PasswordComplexitySetting Setting { get; set; }
    }
}
