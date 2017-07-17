using System.ComponentModel.DataAnnotations;

namespace Derrick.Friendships.Dto
{
    /// <summary>
    /// 阻止用户Input
    /// </summary>
    public class BlockUserInput 
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