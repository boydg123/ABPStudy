using System.ComponentModel.DataAnnotations;

namespace Derrick.Organizations.Dto
{
    /// <summary>
    /// 用户到组织Input
    /// </summary>
    public class UserToOrganizationUnitInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}