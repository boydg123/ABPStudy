using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 标识系统中的一个用户
    /// </summary>
    public class User : AbpUser<User>
    {
        /// <summary>
        /// 密码最小长度
        /// </summary>
        public const int MinPlainPasswordLength = 6;
        /// <summary>
        /// 电话号码最大长度 
        /// </summary>
        public const int MaxPhoneNumberLength = 24;
        /// <summary>
        /// 头像ID
        /// </summary>
        public virtual Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 在下次登录时是否必须修改密码
        /// </summary>
        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        //Can add application specific user properties here
        //可以在此添加用户的特定属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// 为商户创建管理员用户<see cref="User"/>
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="emailAddress">电子邮件</param>
        /// <param name="password">密码</param>
        /// <returns>创建的用户<see cref="User"/>对象</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
                   {
                       TenantId = tenantId,
                       UserName = AdminUserName,
                       Name = AdminUserName,
                       Surname = AdminUserName,
                       EmailAddress = emailAddress,
                       Password = new PasswordHasher().HashPassword(password)
                   };
        }
        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
        /// <summary>
        /// 解锁
        /// </summary>
        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }
    }
}