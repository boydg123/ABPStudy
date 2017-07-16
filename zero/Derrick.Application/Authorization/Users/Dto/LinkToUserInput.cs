using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 链接到用户Input
    /// </summary>
    public class LinkToUserInput 
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 用户名或密码
        /// </summary>
        [Required]
        public string UsernameOrEmailAddress { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}