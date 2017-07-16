using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Roles.Dto;

namespace Derrick.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// 将在角色管理界面中使用的应用程序服务
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {
        /// <summary>
        /// 获取角色Dto列表
        /// </summary>
        /// <param name="input">角色Input</param>
        /// <returns></returns>
        Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input);
        /// <summary>
        /// 获取编辑角色Output
        /// </summary>
        /// <param name="input">空ID Dto</param>
        /// <returns></returns>
        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input);
        /// <summary>
        /// 创建或更新角色
        /// </summary>
        /// <param name="input">创建或更新角色Input</param>
        /// <returns></returns>
        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteRole(EntityDto input);
    }
}