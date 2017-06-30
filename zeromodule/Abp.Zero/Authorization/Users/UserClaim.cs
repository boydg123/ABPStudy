using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// ABP 用户声明
    /// </summary>
    [Table("AbpUserClaims")]
    public class UserClaim : CreationAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 声明类型
        /// </summary>
        public virtual string ClaimType { get; set; }
        /// <summary>
        /// 声明值
        /// </summary>
        public virtual string ClaimValue { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserClaim()
        {
            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="claim">声明对象</param>
        public UserClaim(AbpUserBase user, Claim claim)
        {
            TenantId = user.TenantId;
            UserId = user.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
