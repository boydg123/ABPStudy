using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Permissions.Dto;

namespace Derrick.Authorization.Roles.Dto
{
    /// <summary>
    /// 获取编辑角色Output
    /// </summary>
    public class GetRoleForEditOutput
    {
        /// <summary>
        /// 编辑角色Dto
        /// </summary>
        public RoleEditDto Role { get; set; }
        /// <summary>
        /// 权限Dto列表
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }
        /// <summary>
        /// 授予权限名称集合
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}