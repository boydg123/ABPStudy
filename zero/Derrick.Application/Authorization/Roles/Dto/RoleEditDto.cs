using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Derrick.Authorization.Roles.Dto
{
    /// <summary>
    /// 编辑角色Dto
    /// </summary>
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}