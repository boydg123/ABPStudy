using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Derrick.Authorization.Users.Profile.Dto
{
    /// <summary>
    /// 当前用户资料编辑Dto
    /// </summary>
    [AutoMap(typeof(User))]
    public class CurrentUserProfileEditDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// 姓
        /// </summary>
        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Required]
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(User.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }
    }
}