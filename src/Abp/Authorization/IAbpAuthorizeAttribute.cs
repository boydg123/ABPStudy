namespace Abp.Authorization
{
    /// <summary>
    /// Defines standard interface for authorization attributes.
    /// 为授权特性定义标准接口
    /// </summary>
    public interface IAbpAuthorizeAttribute
    {
        /// <summary>
        /// A list of permissions to authorize.
        /// 需要授权验证的权限列表
        /// </summary>
        string[] Permissions { get; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.
        /// 如果这个属性设置为true，<see cref="Permissions"/>中所有的权限都必须授予。
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// 如果这个属性设置为false，<see cref="Permissions"/>中只需有一个权限被授予便可。
        /// Default: false.
        /// </summary>
        bool RequireAllPermissions { get; set; }
    }
}