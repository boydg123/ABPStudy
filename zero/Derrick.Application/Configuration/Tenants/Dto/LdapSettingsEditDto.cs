namespace Derrick.Configuration.Tenants.Dto
{
    /// <summary>
    /// Ldap设置编辑Dto
    /// </summary>
    public class LdapSettingsEditDto
    {
        /// <summary>
        /// 是否启用模块
        /// </summary>
        public bool IsModuleEnabled { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}