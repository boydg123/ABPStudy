using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Base class for user.
    /// 用户基类
    /// </summary>
    [Table("AbpUsers")]
    public abstract class AbpUserBase : FullAuditedEntity<long>, IUser<long>, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="UserName"/>属性最大长度
        /// </summary>
        public const int MaxUserNameLength = 32;

        /// <summary>
        /// <see cref="EmailAddress"/>属性最大长度
        /// </summary>
        public const int MaxEmailAddressLength = 256;

        /// <summary>
        /// User name.User name must be unique for it's tenant.
        /// 用户名。在同一个商户中用户名必须唯一
        /// </summary>
        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Tenant Id of this user.
        /// 此用户的商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Email address of the user.Email address must be unique for it's tenant.
        /// 用户的邮箱地址。同一个商户的用户邮件地址必须唯一
        /// </summary>
        [Required]
        [StringLength(MaxEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// The last time this user entered to the system.
        /// 当前用户最后一次登入系统的时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from this User.
        /// 构造函数
        /// </summary>
        /// <returns></returns>
        public virtual UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, Id);
        }
    }
}