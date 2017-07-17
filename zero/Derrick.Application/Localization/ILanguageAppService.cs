using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Localization.Dto;

namespace Derrick.Localization
{
    /// <summary>
    /// 语言服务
    /// </summary>
    public interface ILanguageAppService : IApplicationService
    {
        /// <summary>
        /// 获取语言
        /// </summary>
        /// <returns></returns>
        Task<GetLanguagesOutput> GetLanguages();
        /// <summary>
        /// 获取编辑语言
        /// </summary>
        /// <param name="input">空ID Dto</param>
        /// <returns></returns>
        Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input);
        /// <summary>
        /// 创建或更新语言
        /// </summary>
        /// <param name="input">创建或更新语言Input</param>
        /// <returns></returns>
        Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input);
        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task DeleteLanguage(EntityDto input);
        /// <summary>
        /// 设置默认语言
        /// </summary>
        /// <param name="input">设置默认语言Input</param>
        /// <returns></returns>
        Task SetDefaultLanguage(SetDefaultLanguageInput input);
        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="input">获取语言文本Input</param>
        /// <returns></returns>
        Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input);
        /// <summary>
        /// 更新语言文本
        /// </summary>
        /// <param name="input">更新语言文本Input</param>
        /// <returns></returns>
        Task UpdateLanguageText(UpdateLanguageTextInput input);
    }
}
