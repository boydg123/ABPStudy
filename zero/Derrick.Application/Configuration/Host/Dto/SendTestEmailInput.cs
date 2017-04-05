using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Users;

namespace Derrick.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}