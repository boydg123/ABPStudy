using Abp.Auditing;
using Derrick.Authorization.Users;

namespace Derrick.Auditing
{
    /// <summary>
    /// A helper class to store an <see cref="AuditLog"/> and a <see cref="User"/> object.
    /// 存储<see cref="AuditLog"/>和<see cref="User"/>对象的帮助类
    /// </summary>
    public class AuditLogAndUser
    {
        /// <summary>
        /// 审计日志实体
        /// </summary>
        public AuditLog AuditLog { get; set; }
        /// <summary>
        /// 用户实体
        /// </summary>
        public User User { get; set; }
    }
}