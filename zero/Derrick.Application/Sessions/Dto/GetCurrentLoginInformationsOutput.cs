namespace Derrick.Sessions.Dto
{
    /// <summary>
    /// 获取当前登录信息Output
    /// </summary>
    public class GetCurrentLoginInformationsOutput
    {
        /// <summary>
        /// 用户登录信息Dto
        /// </summary>
        public UserLoginInfoDto User { get; set; }
        /// <summary>
        /// 商户登录信息Dto
        /// </summary>
        public TenantLoginInfoDto Tenant { get; set; }
    }
}