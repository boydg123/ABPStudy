namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 宿主用户管理设置编辑Dto
    /// </summary>
    public class HostUserManagementSettingsEditDto
    {
        /// <summary>
        /// 登录时是否需要电子邮件确认
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
    }
}