using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Users;

namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 发送测试邮件Input
    /// </summary>
    public class SendTestEmailInput
    {
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}