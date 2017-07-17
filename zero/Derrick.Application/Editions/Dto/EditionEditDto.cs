using System.ComponentModel.DataAnnotations;
using Abp.Application.Editions;
using Abp.AutoMapper;

namespace Derrick.Editions.Dto
{
    /// <summary>
    /// ∞Ê±æ±‡º≠Dto
    /// </summary>
    [AutoMap(typeof(Edition))]
    public class EditionEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// œ‘ æ√˚
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
    }
}