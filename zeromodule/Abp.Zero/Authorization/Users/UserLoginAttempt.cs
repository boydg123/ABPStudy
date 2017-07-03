using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Abp.Timing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to save a login attempt of a user.
    /// 用于保存用户登录信息
    /// </summary>
    [Table("AbpUserLoginAttempts")]
    public class UserLoginAttempt : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="TenancyName"/>属性的最大长度
        /// </summary>
        public const int MaxTenancyNameLength = AbpTenantBase.MaxTenancyNameLength;

        /// <summary>
        /// <see cref="UserNameOrEmailAddress"/>属性的最大长度
        /// </summary>
        public const int MaxUserNameOrEmailAddressLength = 255;

        /// <summary>
        /// <see cref="ClientIpAddress"/>属性的最大长度
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// <see cref="ClientName"/>属性的最大长度
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// <see cref="BrowserInfo"/>属性的最大长度
        /// </summary>
        public const int MaxBrowserInfoLength = 256;

        /// <summary>
        /// Tenant's Id, if <see cref="TenancyName"/> was a valid tenant name.
        /// 商户ID，如果<see cref="TenancyName"/>是一个有效的商户名称
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Tenancy name.
        /// 商户名称
        /// </summary>
        [MaxLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// User's Id, if <see cref="UserNameOrEmailAddress"/> was a valid username or email address.
        /// 用户ID，如果<see cref="UserNameOrEmailAddress"/>是一个有效的用户名或邮箱地址
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// User name or email address
        /// 用户名或邮箱地址
        /// </summary>
        [MaxLength(MaxUserNameOrEmailAddressLength)]
        public virtual string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// IP address of the client.
        /// 客户端的邮箱地址
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// 客户机的名称(一般是计算机名)
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// 如果在Web请求中调用此方法，则为浏览器信息
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// Login attempt result.
        /// 登录尝试的结构
        /// </summary>
        public virtual AbpLoginResultType Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserLoginAttempt()
        {
            CreationTime = Clock.Now;
        }
    }
}
