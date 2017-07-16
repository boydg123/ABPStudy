using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 更新用户权限Input
    /// </summary>
    public class UpdateUserPermissionsInput 
    {
        /// <summary>
        /// ID
        /// </summary>
        [Range(1, int.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 授予权限名称列表
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}