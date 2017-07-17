using System.ComponentModel.DataAnnotations;

namespace Derrick.Organizations.Dto
{
    /// <summary>
    /// 移动组织架构Input
    /// </summary>
    public class MoveOrganizationUnitInput
    {
        /// <summary>
        /// ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
        /// <summary>
        /// 新父ID
        /// </summary>
        public long? NewParentId { get; set; }
    }
}