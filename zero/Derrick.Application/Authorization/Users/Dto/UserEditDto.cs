using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Entities;

namespace Derrick.Authorization.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    /// <summary>
    /// 在自定义Dto映射中与用户映射
    /// </summary>
    public class UserEditDto : IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// 设置为Null则创建用户，设置用户ID则更新用户
        /// </summary>
        public long? Id { get; set; }
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
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(User.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 密码(不需要设置'必须'属性，空值用来不修改密码)
        /// </summary>
        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否在下次登录时修改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }
        /// <summary>
        /// 是否开启双因子
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }
        /// <summary>
        /// 是否开启锁定
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }
    }
}