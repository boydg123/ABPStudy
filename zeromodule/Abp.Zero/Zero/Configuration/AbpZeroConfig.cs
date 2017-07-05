namespace Abp.Zero.Configuration
{
    /// <summary>
    /// ABP Zero配置
    /// </summary>
    internal class AbpZeroConfig : IAbpZeroConfig
    {
        /// <summary>
        /// 角色管理配置
        /// </summary>
        public IRoleManagementConfig RoleManagement
        {
            get { return _roleManagementConfig; }
        }
        private readonly IRoleManagementConfig _roleManagementConfig;
        /// <summary>
        /// 用户管理配置
        /// </summary>
        public IUserManagementConfig UserManagement
        {
            get { return _userManagementConfig; }
        }
        private readonly IUserManagementConfig _userManagementConfig;
        /// <summary>
        /// 语言管理配置
        /// </summary>
        public ILanguageManagementConfig LanguageManagement
        {
            get { return _languageManagement; }
        }
        private readonly ILanguageManagementConfig _languageManagement;
        /// <summary>
        /// 实体类型
        /// </summary>
        public IAbpZeroEntityTypes EntityTypes
        {
            get { return _entityTypes; }
        }
        private readonly IAbpZeroEntityTypes _entityTypes;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleManagementConfig">角色管理配置</param>
        /// <param name="userManagementConfig">用户管理配置</param>
        /// <param name="languageManagement">语言管理配置</param>
        /// <param name="entityTypes">实体类型</param>
        public AbpZeroConfig(
            IRoleManagementConfig roleManagementConfig,
            IUserManagementConfig userManagementConfig,
            ILanguageManagementConfig languageManagement,
            IAbpZeroEntityTypes entityTypes)
        {
            _entityTypes = entityTypes;
            _roleManagementConfig = roleManagementConfig;
            _userManagementConfig = userManagementConfig;
            _languageManagement = languageManagement;
        }
    }
}