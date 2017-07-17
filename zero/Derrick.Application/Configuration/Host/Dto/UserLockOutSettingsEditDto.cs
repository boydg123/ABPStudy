namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 用户锁定设置编辑Dto
    /// </summary>
    public class UserLockOutSettingsEditDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 锁定前最大失败访问尝试次数
        /// </summary>
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
        /// <summary>
        /// 默认账户锁定秒数
        /// </summary>
        public int DefaultAccountLockoutSeconds { get; set; }
    }
}