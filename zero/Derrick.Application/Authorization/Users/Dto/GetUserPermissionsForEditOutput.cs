using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 用户权限编辑Output
    /// </summary>
    public class GetUserPermissionsForEditOutput
    {
        /// <summary>
        /// 统一权限Dto
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }
        /// <summary>
        /// 授予权限名称列表
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}