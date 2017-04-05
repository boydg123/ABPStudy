using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
