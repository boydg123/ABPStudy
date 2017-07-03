namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to store setting for a permission for a user.
    /// 用于存储用户的权限设置
    /// </summary>
    public class UserPermissionSetting : PermissionSetting
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }
    }
}