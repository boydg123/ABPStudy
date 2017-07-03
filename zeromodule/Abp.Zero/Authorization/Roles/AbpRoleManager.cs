using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Zero;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.Applications should derive this class with appropriate generic arguments.
    /// ASP.NET Identity框架的扩展<see cref="RoleManager{TRole,TKey}"/> .应用程序应该用适当的泛型参数派生这个类
    /// </summary>
    public abstract class AbpRoleManager<TRole, TUser>
        : RoleManager<TRole, int>,
        IDomainService
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 本地化管理引用
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }
        /// <summary>
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 角色管理配置
        /// </summary>
        public IRoleManagementConfig RoleManagementConfig { get; private set; }
        /// <summary>
        /// 角色权限存储引用
        /// </summary>
        private IRolePermissionStore<TRole, TUser> RolePermissionStore
        {
            get
            {
                if (!(Store is IRolePermissionStore<TRole, TUser>))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }

                return Store as IRolePermissionStore<TRole, TUser>;
            }
        }
        /// <summary>
        /// 角色存储引用
        /// </summary>
        protected AbpRoleStore<TRole, TUser> AbpStore { get; private set; }
        /// <summary>
        /// 权限管理引用
        /// </summary>
        private readonly IPermissionManager _permissionManager;
        /// <summary>
        /// 缓存引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpRoleManager(
            AbpRoleStore<TRole, TUser> store,
            IPermissionManager permissionManager,
            IRoleManagementConfig roleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(store)
        {
            _permissionManager = permissionManager;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;

            RoleManagementConfig = roleManagementConfig;
            AbpStore = store;
            AbpSession = NullAbpSession.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        #region Obsolete methods

        /// <summary>
        /// Checks if a role has a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission / 检查它权限的角色名称</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        [Obsolete("Use IsGrantedAsync instead.")]
        public virtual async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await IsGrantedAsync((await GetRoleByNameAsync(roleName)).Id, _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission / 检查它权限的角色ID</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        [Obsolete("Use IsGrantedAsync instead.")]
        public virtual async Task<bool> HasPermissionAsync(int roleId, string permissionName)
        {
            return await IsGrantedAsync(roleId, _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="role">The role / 角色对象</param>
        /// <param name="permission">The permission / 权限对象</param>
        /// <returns>True, if the role has the permission /  true,如果角色拥有此权限</returns>
        [Obsolete("Use IsGrantedAsync instead.")]
        public Task<bool> HasPermissionAsync(TRole role, Permission permission)
        {
            return IsGrantedAsync(role.Id, permission);
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permission">权限对象</param>
        /// <returns>True, if the role has the permission /  true,如果角色拥有此权限</returns>
        [Obsolete("Use IsGrantedAsync instead.")]
        public Task<bool> HasPermissionAsync(int roleId, Permission permission)
        {
            return IsGrantedAsync(roleId, permission);
        }

        #endregion

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission / 检查它权限的角色名称</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        public virtual async Task<bool> IsGrantedAsync(string roleName, string permissionName)
        {
            return await IsGrantedAsync((await GetRoleByNameAsync(roleName)).Id, _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// 检查角色是否有给定的权限
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission / 检查它权限的角色ID</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        public virtual async Task<bool> IsGrantedAsync(int roleId, string permissionName)
        {
            return await IsGrantedAsync(roleId, _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// 检查某个角色是否被授予权限
        /// </summary>
        /// <param name="role">The role / 角色</param>
        /// <param name="permission">The permission / 权限</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        public Task<bool> IsGrantedAsync(TRole role, Permission permission)
        {
            return IsGrantedAsync(role.Id, permission);
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// 检查某个角色是否被授予权限
        /// </summary>
        /// <param name="roleId">role id / 角色ID</param>
        /// <param name="permission">The permission / 权限对象</param>
        /// <returns>True, if the role has the permission / true,如果角色拥有此权限</returns>
        public virtual async Task<bool> IsGrantedAsync(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// 获取角色的授予权限集合
        /// </summary>
        /// <param name="roleId">Role id / 角色ID</param>
        /// <returns>List of granted permissions / 授予的权限集合</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// 获取角色的授予权限集合
        /// </summary>
        /// <param name="roleName">Role name / 角色名称</param>
        /// <returns>List of granted permissions / 授予权限集合</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByNameAsync(roleName));
        }

        /// <summary>
        /// Gets granted permissions for a role.
        /// 获取角色的授予权限集合
        /// </summary>
        /// <param name="role">Role / 角色对象</param>
        /// <returns>List of granted permissions / 授予的权限集合</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TRole role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(role.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.Prohibits all other permissions.
        /// 立即设置角色的所有授予权限，禁止所有的其他权限
        /// </summary>
        /// <param name="roleId">Role id / 角色ID</param>
        /// <param name="permissions">Permissions / 权限集合</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.Prohibits all other permissions.
        /// 立即设置角色的所有授予权限，禁止所有的其他权限
        /// </summary>
        /// <param name="role">The role / 角色对象</param>
        /// <param name="permissions">Permissions / 权限集合</param>
        public virtual async Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p, PermissionEqualityComparer.Instance)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p, PermissionEqualityComparer.Instance)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Grants a permission for a role.
        /// 为角色授予一个权限
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <param name="permission">权限对象</param>
        public async Task GrantPermissionAsync(TRole role, Permission permission)
        {
            if (await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// Prohibits a permission for a role.
        /// 为角色禁止一个权限
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="permission">权限</param>
        public async Task ProhibitPermissionAsync(TRole role, Permission permission)
        {
            if (!await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// Prohibits all permissions for a role.
        /// 为角色禁止所有权限
        /// </summary>
        /// <param name="role">角色</param>
        public async Task ProhibitAllPermissionsAsync(TRole role)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a role.It removes all permission settings for the role.
        /// 为角色重置所有权限，它将为角色移除所有的权限设置
        /// Role will have permissions those have <see cref="Permission.IsGrantedByDefault"/> set to true.
        /// 角色将有设置<see cref="Permission.IsGrantedByDefault"/>为true的权限
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ResetAllPermissionsAsync(TRole role)
        {
            await RolePermissionStore.RemoveAllPermissionSettingsAsync(role);
        }

        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="role">角色</param>
        public override async Task<IdentityResult> CreateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            if (!result.Succeeded)
            {
                return result;
            }

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !role.TenantId.HasValue)
            {
                role.TenantId = tenantId.Value;
            }

            return await base.CreateAsync(role);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public override async Task<IdentityResult> UpdateAsync(TRole role)
        {
            var result = await CheckDuplicateRoleNameAsync(role.Id, role.Name, role.DisplayName);
            if (!result.Succeeded)
            {
                return result;
            }

            return await base.UpdateAsync(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role">角色</param>
        public async override Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role.IsStatic)
            {
                return AbpIdentityResult.Failed(string.Format(L("CanNotDeleteStaticRole"), role.Name));
            }

            return await base.DeleteAsync(role);
        }

        /// <summary>
        /// Gets a role by given id.Throws exception if no role with given id.
        /// 通过给定的ID获取角色，如果没有则抛出异常
        /// </summary>
        /// <param name="roleId">Role id / 角色ID</param>
        /// <returns>角色</returns>
        /// <exception cref="AbpException">如果没有找到则抛出的异常对象</exception>
        public virtual async Task<TRole> GetRoleByIdAsync(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id: " + roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets a role by given name.Throws exception if no role with given roleName.
        /// 通过给定的名称获取角色，如果没有找到则抛出异常
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>角色对象</returns>
        /// <exception cref="AbpException">如果角色名称没有找到对应的角色则抛出的异常对象</exception>
        public virtual async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name: " + roleName);
            }

            return role;
        }
        /// <summary>
        /// 为角色授予所有权限
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public async Task GrantAllPermissionsAsync(TRole role)
        {
            var permissions = _permissionManager.GetAllPermissions(role.GetMultiTenancySide());
            await SetGrantedPermissionsAsync(role, permissions);
        }
        /// <summary>
        /// 创建静态角色
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IdentityResult> CreateStaticRoles(int tenantId)
        {
            var staticRoleDefinitions = RoleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant);

            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                foreach (var staticRoleDefinition in staticRoleDefinitions)
                {
                    var role = new TRole
                    {
                        TenantId = tenantId,
                        Name = staticRoleDefinition.RoleName,
                        DisplayName = staticRoleDefinition.RoleName,
                        IsStatic = true
                    };

                    var identityResult = await CreateAsync(role);
                    if (!identityResult.Succeeded)
                    {
                        return identityResult;
                    }
                }
            }

            return IdentityResult.Success;
        }
        /// <summary>
        /// 检查重复的角色名称
        /// </summary>
        /// <param name="expectedRoleId">预计的角色ID</param>
        /// <param name="name">角色名称</param>
        /// <param name="displayName">显示名称</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> CheckDuplicateRoleNameAsync(int? expectedRoleId, string name, string displayName)
        {
            var role = await FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                return AbpIdentityResult.Failed(string.Format(L("RoleNameIsAlreadyTaken"), name));
            }

            role = await FindByDisplayNameAsync(displayName);
            if (role != null && role.Id != expectedRoleId)
            {
                return AbpIdentityResult.Failed(string.Format(L("RoleDisplayNameIsAlreadyTaken"), displayName));
            }

            return IdentityResult.Success;
        }
        /// <summary>
        /// 通过显示名查找角色
        /// </summary>
        /// <param name="displayName">显示名</param>
        /// <returns>角色对象</returns>
        private Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return AbpStore.FindByDisplayNameAsync(displayName);
        }
        /// <summary>
        /// 获取角色权限缓存项
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        private async Task<RolePermissionCacheItem> GetRolePermissionCacheItemAsync(int roleId)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
            return await _cacheManager.GetRolePermissionCache().GetAsync(cacheKey, async () =>
            {
                var newCacheItem = new RolePermissionCacheItem(roleId);

                foreach (var permissionInfo in await RolePermissionStore.GetPermissionsAsync(roleId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.Add(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }
        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="name">获取字符串Key</param>
        /// <returns></returns>
        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }
        /// <summary>
        /// 获取当前商户ID
        /// </summary>
        /// <returns></returns>
        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}