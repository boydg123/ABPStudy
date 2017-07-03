using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Implements 'Role Store' of ASP.NET Identity Framework.
    /// 使用ASP.NET Identity框架实现'角色存储'
    /// </summary>
    public abstract class AbpRoleStore<TRole, TUser> :
        IQueryableRoleStore<TRole, int>,
        IRolePermissionStore<TRole, TUser>,

        ITransientDependency

        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 角色仓储
        /// </summary>
        private readonly IRepository<TRole> _roleRepository;
        /// <summary>
        /// 用户角色仓储
        /// </summary>
        private readonly IRepository<UserRole, long> _userRoleRepository;
        /// <summary>
        /// 角色权限设置仓储
        /// </summary>
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpRoleStore(
            IRepository<TRole> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }
        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        public virtual IQueryable<TRole> Roles
        {
            get { return _roleRepository.GetAll(); }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TRole role)
        {
            await _roleRepository.InsertAsync(role);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TRole role)
        {
            await _roleRepository.UpdateAsync(role);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TRole role)
        {
            await _userRoleRepository.DeleteAsync(ur => ur.RoleId == role.Id);
            await _roleRepository.DeleteAsync(role);
        }
        /// <summary>
        /// 通过角色ID查找角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public virtual async Task<TRole> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }
        /// <summary>
        /// 通过角色名称查找角色信息
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public virtual async Task<TRole> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.Name == roleName
                );
        }
        /// <summary>
        /// 通过角色显示名称查找角色信息
        /// </summary>
        /// <param name="displayName">角色显示名</param>
        /// <returns></returns>
        public virtual async Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.DisplayName == displayName
                );
        }

        /// <summary>
        /// 为角色添加一个全选授予设置
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <param name="permissionGrant">权限授予信息</param>
        public virtual async Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role.Id, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new RolePermissionSetting
                {
                    TenantId = role.TenantId,
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        /// <summary>
        /// 为角色移除一个权限授予设置
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="permissionGrant">权限授予信息</param>
        public virtual async Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }
        /// <summary>
        /// 为角色获取权限授予设置信息集合
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>权限设置信息集合</returns>
        public virtual Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            return GetPermissionsAsync(role.Id);
        }
        /// <summary>
        /// 为角色获取权限授予设置信息集合
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限设置信息集合</returns>
        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == roleId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        /// <summary>
        /// 检查角色是否有权限授予设置信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionGrant">权限授予设置信息</param>
        /// <returns></returns>
        public virtual async Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == roleId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        /// <summary>
        /// 为角色删除所有权限设置
        /// </summary>
        /// <param name="role">角色</param>
        public virtual async Task RemoveAllPermissionSettingsAsync(TRole role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
