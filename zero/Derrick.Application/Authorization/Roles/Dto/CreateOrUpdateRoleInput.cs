using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Roles.Dto
{
    /// <summary>
    /// 创建或更新角色Input
    /// </summary>
    public class CreateOrUpdateRoleInput 
    {
        /// <summary>
        /// 角色编辑Dto
        /// </summary>
        [Required]
        public RoleEditDto Role { get; set; }
        /// <summary>
        /// 授予权限名称列表
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}