namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Used to store setting for a permission for a role.
    /// 用于为角色存储权限设置
    /// </summary>
    public class RolePermissionSetting : PermissionSetting
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}