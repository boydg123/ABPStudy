using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Derrick.Authorization.Permissions;
using Derrick.Authorization.Permissions.Dto;
using Derrick.Authorization.Roles.Dto;

namespace Derrick.Authorization.Roles
{
    /// <summary>
    /// <see cref="IRoleAppService"/>实现。将在角色管理界面中使用的应用程序服务
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_Roles)]
    public class RoleAppService : AbpZeroTemplateAppServiceBase, IRoleAppService
    {
        /// <summary>
        /// 角色管理
        /// </summary>
        private readonly RoleManager _roleManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleManager">角色管理</param>
        public RoleAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }
        /// <summary>
        /// 获取角色列表Dto集合
        /// </summary>
        /// <param name="input">获取角色Input</param>
        /// <returns></returns>
        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(roles.MapTo<List<RoleListDto>>());
        }
        /// <summary>
        /// 获取编辑角色Dto
        /// </summary>
        /// <param name="input">空ID Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = role.MapTo<RoleEditDto>();
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
        /// <summary>
        /// 创建或更新角色
        /// </summary>
        /// <param name="input">创建或更新角色Input</param>
        /// <returns></returns>
        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Delete)]
        public async Task DeleteRole(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input">创建或更新角色Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");

            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;

            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input">创建或更新角色Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
        }
        /// <summary>
        /// 更新授予权限
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="grantedPermissionNames">授予权限名称列表</param>
        /// <returns></returns>
        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}
