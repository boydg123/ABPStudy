namespace Abp.Zero.Configuration
{
    /// <summary>
    /// Configuration options for zero module.
    /// Zero模块的配置选项
    /// </summary>
    public interface IAbpZeroConfig
    {
        /// <summary>
        /// Gets role management config.
        /// 获取角色管理配置
        /// </summary>
        IRoleManagementConfig RoleManagement { get; }

        /// <summary>
        /// Gets user management config.
        /// 获取用户管理配置
        /// </summary>
        IUserManagementConfig UserManagement { get; }

        /// <summary>
        /// Gets language management config.
        /// 获取语言管理配置
        /// </summary>
        ILanguageManagementConfig LanguageManagement { get; }

        /// <summary>
        /// Gets entity type config.
        /// 获取实体类型配置
        /// </summary>
        IAbpZeroEntityTypes EntityTypes { get; }
    }
}