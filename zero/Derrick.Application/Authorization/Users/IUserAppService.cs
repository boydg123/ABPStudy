using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Users.Dto;
using Derrick.Dto;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户APP服务
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户列表Dto(带分页)
        /// </summary>
        /// <param name="input">用户输入信息</param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);
        /// <summary>
        /// 获取导出到Excel的用户信息
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetUsersToExcel();
        /// <summary>
        /// 获取编辑时的用户信息
        /// </summary>
        /// <param name="input">可空的ID Dto</param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);
        /// <summary>
        /// 获取编辑时的用户权限信息
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);
        /// <summary>
        /// 重置用户特定权限
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task ResetUserSpecificPermissions(EntityDto<long> input);
        /// <summary>
        /// 更新用户权限
        /// </summary>
        /// <param name="input">更新用户权限输入信息</param>
        /// <returns></returns>
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);
        /// <summary>
        /// 创建或更新用户
        /// </summary>
        /// <param name="input">创建或更新用户输入信息</param>
        /// <returns></returns>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteUser(EntityDto<long> input);
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task UnlockUser(EntityDto<long> input);
    }
}