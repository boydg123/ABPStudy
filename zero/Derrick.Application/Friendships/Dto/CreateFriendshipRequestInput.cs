using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Friendships.Dto
{
    /// <summary>
    /// 创建好友请求Input
    /// </summary>
    public class CreateFriendshipRequestInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
    }
}