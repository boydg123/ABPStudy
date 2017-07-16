using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;

namespace Derrick.Authorization.Users.Profile.Dto
{
    /// <summary>
    /// 修改密码Input
    /// </summary>
    public class ChangePasswordInput
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}