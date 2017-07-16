using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 用户角色Dto
    /// </summary>
    public class UserRoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色显示名
        /// </summary>
        public string RoleDisplayName { get; set; }
        /// <summary>
        /// 是否被分配
        /// </summary>
        public bool IsAssigned { get; set; }
    }
}