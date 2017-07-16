using Abp;
using Abp.Application.Services.Dto;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 用户断开链接Input
    /// </summary>
    public class UnlinkUserInput
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 转换成用户标识
        /// </summary>
        /// <returns></returns>
        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }
    }
}