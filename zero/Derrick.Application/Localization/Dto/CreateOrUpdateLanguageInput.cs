using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}