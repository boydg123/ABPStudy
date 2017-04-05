using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}