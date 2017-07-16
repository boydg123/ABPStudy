namespace Abp.Authorization.Users
{
    /// <summary>
    /// ABP登录结果类型
    /// </summary>
    public enum AbpLoginResultType : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 无效的电子邮件或用户名
        /// </summary>
        InvalidUserNameOrEmailAddress,
        /// <summary>
        /// 无效密码
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// 用户未激活
        /// </summary>
        UserIsNotActive,
        /// <summary>
        /// 无效的商户名
        /// </summary>
        InvalidTenancyName,
        /// <summary>
        /// 商户未激活
        /// </summary>
        TenantIsNotActive,
        /// <summary>
        /// 用户邮件未确认
        /// </summary>
        UserEmailIsNotConfirmed,
        /// <summary>
        /// 未知的外部登录
        /// </summary>
        UnknownExternalLogin,
        /// <summary>
        /// 锁定
        /// </summary>
        LockedOut
    }
}