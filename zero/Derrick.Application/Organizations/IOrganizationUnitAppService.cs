using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Organizations.Dto;

namespace Derrick.Organizations
{
    /// <summary>
    /// 组织架构服务
    /// </summary>
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits();
        /// <summary>
        /// 获取组织架构用户
        /// </summary>
        /// <param name="input">获取组织架构用户Input</param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input);
        /// <summary>
        /// 创建组织架构
        /// </summary>
        /// <param name="input">创建组织架构Input</param>
        /// <returns></returns>
        Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input);
        /// <summary>
        /// 更新组织架构
        /// </summary>
        /// <param name="input">更新组织架构Input</param>
        /// <returns></returns>
        Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input);
        /// <summary>
        /// 转移组织架构
        /// </summary>
        /// <param name="input">转移组织架构Input</param>
        /// <returns></returns>
        Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input);
        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteOrganizationUnit(EntityDto<long> input);
        /// <summary>
        /// 添加用户到组织架构
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        Task AddUserToOrganizationUnit(UserToOrganizationUnitInput input);
        /// <summary>
        /// 从组织架构移除用户
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input);
        /// <summary>
        /// 判断用户是否在组织中
        /// </summary>
        /// <param name="input">用户到组织架构Input</param>
        /// <returns></returns>
        Task<bool> IsInOrganizationUnit(UserToOrganizationUnitInput input);
    }
}
