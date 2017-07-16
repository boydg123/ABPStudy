using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
