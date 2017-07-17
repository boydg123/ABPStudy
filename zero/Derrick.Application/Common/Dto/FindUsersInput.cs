using Derrick.Dto;

namespace Derrick.Common.Dto
{
    /// <summary>
    /// 查找用户Input
    /// </summary>
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
    }
}