using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// 表示一个用户
    /// </summary>
    public abstract class AbpUser<TUser> : AbpUserBase, IFullAudited<TUser>, IPassivable
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 管理员的用户名。管理员不能被删除并且管理员的用户名不能被修改。
        /// </summary>
        public const string AdminUserName = "admin";

        /// <summary>
        /// <see cref="Name"/>属性最大长度
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// <see cref="Surname"/>属性最大长度
        /// </summary>
        public const int MaxSurnameLength = 32;

        /// <summary>
        /// <see cref="Password"/>属性最大长度
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        /// <see cref="Password"/>属性最大长度
        /// </summary>
        public const int MaxPlainPasswordLength = 32;

        /// <summary>
        /// <see cref="EmailConfirmationCode"/>属性最大长度
        /// </summary>
        public const int MaxEmailConfirmationCodeLength = 328;

        /// <summary>
        /// <see cref="PasswordResetCode"/>属性最大长度
        /// </summary>
        public const int MaxPasswordResetCodeLength = 328;

        /// <summary>
        /// <see cref="AuthenticationSource"/>属性最大长度
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

        /// <summary>
        /// Authorization source name.It's set to external authentication source name if created by an external source.Default: null.
        /// 授权源名称。如果由外部源创建，则将其设置为外部身份验证源名称。默认值：null
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// Name of the user.
        /// 用户的名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// 用户的姓
        /// </summary>
        [Required]
        [StringLength(MaxSurnameLength)]
        public virtual string Surname { get; set; }

        /// <summary>
        /// Return full name (Name Surname )
        /// 全名
        /// </summary>
        [NotMapped]
        public virtual string FullName { get { return this.Name + " " + this.Surname; } }

        /// <summary>
        /// Password of the user.
        /// 用户的密码
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Is the <see cref="AbpUserBase.EmailAddress"/> confirmed.
        /// 用户的<see cref="AbpUserBase.EmailAddress"/>是否确认
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// 电子邮件确认码
        /// </summary>
        [StringLength(MaxEmailConfirmationCodeLength)]
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.It's not valid if it's null.It's for one usage and must be set to null after reset.
        /// 重置密码的Code。如果它是null则无效。它只能用一次，重置后必须设置为null
        /// </summary>
        [StringLength(MaxPasswordResetCodeLength)]
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// Lockout end date.
        /// 锁定的最后日期
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// 访问失败的次数
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the lockout enabled.
        /// 锁定是否开启
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// 电话号
        /// </summary>
        public virtual string PhoneNumber {get; set; }

        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// <see cref="PhoneNumber"/>是否确认
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// 安全标记
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor auth enabled.
        /// 是否启用双因素身份验证
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }

        /// <summary>
        /// Is this user active?If as user is not active, he/she can not use the application.
        /// 是否是活动用户，如果不是活动状态，则他不能使用应用程序
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Login definitions for this user.
        /// 此用户的登录定义
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserLogin> Logins { get; set; }

        /// <summary>
        /// Roles of this user.
        /// 此用户的角色列表
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// Claims of this user.
        /// 此用户的Claims
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserClaim> Claims { get; set; }

        /// <summary>
        /// Permission definitions for this user.
        /// 此用户的权限定义
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Settings for this user.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<Setting> Settings { get; set; }

        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }

        protected AbpUser()
        {
            IsActive = true;
            SecurityStamp = SequentialGuidGenerator.Instance.Create().ToString();
        }

        public virtual void SetNewPasswordResetCode()
        {
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(MaxPasswordResetCodeLength);
        }

        public virtual void SetNewEmailConfirmationCode()
        {
            EmailConfirmationCode = Guid.NewGuid().ToString("N").Truncate(MaxEmailConfirmationCodeLength);
        }

        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }
    }
}