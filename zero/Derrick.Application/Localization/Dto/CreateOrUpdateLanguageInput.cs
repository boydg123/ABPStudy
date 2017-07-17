using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Derrick.Localization.Dto
{
    /// <summary>
    /// 创建或更新语言Input
    /// </summary>
    public class CreateOrUpdateLanguageInput
    {
        /// <summary>
        /// 应用程序语言编辑Dto
        /// </summary>
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}