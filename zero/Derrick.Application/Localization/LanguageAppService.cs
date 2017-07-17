using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.UI;
using Derrick.Authorization;
using Derrick.Localization.Dto;

namespace Derrick.Localization
{
    /// <summary>
    /// 语言服务实现
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_Languages)]
    public class LanguageAppService : AbpZeroTemplateAppServiceBase, ILanguageAppService
    {
        /// <summary>
        /// 应用程序语言管理器
        /// </summary>
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        /// <summary>
        /// 应用程序语言文本管理器
        /// </summary>
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        /// <summary>
        /// 应用程序语言仓储
        /// </summary>
        private readonly IRepository<ApplicationLanguage> _languageRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationLanguageManager">应用程序语言管理</param>
        /// <param name="applicationLanguageTextManager">应用程序语言文本管理</param>
        /// <param name="languageRepository">应用程序语言仓储</param>
        public LanguageAppService(
            IApplicationLanguageManager applicationLanguageManager,
            IApplicationLanguageTextManager applicationLanguageTextManager,
            IRepository<ApplicationLanguage> languageRepository)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _languageRepository = languageRepository;
            _applicationLanguageTextManager = applicationLanguageTextManager;
        }

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <returns></returns>
        public async Task<GetLanguagesOutput> GetLanguages()
        {
            var languages = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).OrderBy(l => l.DisplayName);
            var defaultLanguage = await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            return new GetLanguagesOutput(
                languages.MapTo<List<ApplicationLanguageListDto>>(),
                defaultLanguage == null ? null : defaultLanguage.Name
                );
        }
        /// <summary>
        /// 获取编辑语言
        /// </summary>
        /// <param name="input">空ID Dto</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Create, AppPermissions.Pages_Administration_Languages_Edit)]
        public async Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input)
        {
            ApplicationLanguage language = null;
            if (input.Id.HasValue)
            {
                language = await _languageRepository.GetAsync(input.Id.Value);
            }

            var output = new GetLanguageForEditOutput();

            //Language
            output.Language = language != null
                ? language.MapTo<ApplicationLanguageEditDto>()
                : new ApplicationLanguageEditDto();

            //Language names
            output.LanguageNames = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .OrderBy(c => c.DisplayName)
                .Select(c => new ComboboxItemDto(c.Name, c.DisplayName + " (" + c.Name + ")") { IsSelected = output.Language.Name == c.Name })
                .ToList();

            //Flags
            output.Flags = FamFamFamFlagsHelper
                .FlagClassNames
                .OrderBy(f => f)
                .Select(f => new ComboboxItemDto(f, FamFamFamFlagsHelper.GetCountryCode(f)) { IsSelected = output.Language.Icon == f})
                .ToList();

            return output;
        }
        /// <summary>
        /// 创建或更新语言
        /// </summary>
        /// <param name="input">创建或更新语言Input</param>
        /// <returns></returns>
        public async Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input)
        {
            if (input.Language.Id.HasValue)
            {
                await UpdateLanguageAsync(input);
            }
            else
            {
                await CreateLanguageAsync(input);
            }
        }
        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        public async Task DeleteLanguage(EntityDto input)
        {
            var language = await _languageRepository.GetAsync(input.Id);
            await _applicationLanguageManager.RemoveAsync(AbpSession.TenantId, language.Name);
        }
        /// <summary>
        /// 设置默认语言
        /// </summary>
        /// <param name="input">设置默认语言Input</param>
        /// <returns></returns>
        public async Task SetDefaultLanguage(SetDefaultLanguageInput input)
        {
            await _applicationLanguageManager.SetDefaultLanguageAsync(
                AbpSession.TenantId,
                GetCultureInfoByChecking(input.Name).Name
                );
        }
        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="input">获取语言文本Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_ChangeTexts)]
        public async Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input)
        {
            /* Note: This method is used by SPA without paging, MPA with paging.
             * So, it can both usable with paging or not */

            //Normalize base language name
            if (input.BaseLanguageName.IsNullOrEmpty())
            {
                var defaultLanguage = await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);
                if (defaultLanguage == null)
                {
                    defaultLanguage = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).FirstOrDefault();
                    if (defaultLanguage == null)
                    {
                        throw new ApplicationException("No language found in the application!");
                    }
                }

                input.BaseLanguageName = defaultLanguage.Name;
            }

            var source = LocalizationManager.GetSource(input.SourceName);
            var baseCulture = CultureInfo.GetCultureInfo(input.BaseLanguageName);
            var targetCulture = CultureInfo.GetCultureInfo(input.TargetLanguageName);

            var languageTexts = source
                .GetAllStrings()
                .Select(localizedString => new LanguageTextListDto
                {
                    Key = localizedString.Name,
                    BaseValue = _applicationLanguageTextManager.GetStringOrNull(AbpSession.TenantId, source.Name, baseCulture, localizedString.Name),
                    TargetValue = _applicationLanguageTextManager.GetStringOrNull(AbpSession.TenantId, source.Name, targetCulture, localizedString.Name, false)
                })
                .AsQueryable();

            //Filters
            if (input.TargetValueFilter == "EMPTY")
            {
                languageTexts = languageTexts.Where(s => s.TargetValue.IsNullOrEmpty());
            }

            if (!input.FilterText.IsNullOrEmpty())
            {
                languageTexts = languageTexts.Where(
                    l => (l.Key != null && l.Key.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                         (l.BaseValue != null && l.BaseValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                         (l.TargetValue != null && l.TargetValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    );
            }

            var totalCount = languageTexts.Count();

            //Ordering
            if (!input.Sorting.IsNullOrEmpty())
            {
                languageTexts = languageTexts.OrderBy(input.Sorting);
            }

            //Paging
            if (input.SkipCount > 0)
            {
                languageTexts = languageTexts.Skip(input.SkipCount);
            }

            if (input.MaxResultCount > 0)
            {
                languageTexts = languageTexts.Take(input.MaxResultCount);
            }

            return new PagedResultDto<LanguageTextListDto>(
                totalCount,
                languageTexts.ToList()
                );
        }
        /// <summary>
        /// 更新语言文本
        /// </summary>
        /// <param name="input">更新语言文本Input</param>
        /// <returns></returns>
        public async Task UpdateLanguageText(UpdateLanguageTextInput input)
        {
            var culture = GetCultureInfoByChecking(input.LanguageName);
            var source = LocalizationManager.GetSource(input.SourceName);
            await _applicationLanguageTextManager.UpdateStringAsync(AbpSession.TenantId, source.Name, culture, input.Key, input.Value);
        }
        /// <summary>
        /// 创建语言
        /// </summary>
        /// <param name="input">创建或更新语言Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Create)]
        protected virtual async Task CreateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            var culture = GetCultureInfoByChecking(input.Language.Name);

            await CheckLanguageIfAlreadyExists(culture.Name);

            await _applicationLanguageManager.AddAsync(
                new ApplicationLanguage(
                    AbpSession.TenantId,
                    culture.Name,
                    culture.DisplayName,
                    input.Language.Icon
                    )
                );
        }
        /// <summary>
        /// 更新语言
        /// </summary>
        /// <param name="input">创建或更新语言Input</param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Edit)]
        protected virtual async Task UpdateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            Debug.Assert(input.Language.Id != null, "input.Language.Id != null");

            var culture = GetCultureInfoByChecking(input.Language.Name);

            await CheckLanguageIfAlreadyExists(culture.Name, input.Language.Id.Value);

            var language = await _languageRepository.GetAsync(input.Language.Id.Value);

            language.Name = culture.Name;
            language.DisplayName = culture.DisplayName;
            language.Icon = input.Language.Icon;

            await _applicationLanguageManager.UpdateAsync(AbpSession.TenantId, language);
        }
        /// <summary>
        /// 通过检测获取区域信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private CultureInfo GetCultureInfoByChecking(string name)
        {
            try
            {
                return CultureInfo.GetCultureInfo(name);
            }
            catch (CultureNotFoundException ex)
            {
                Logger.Warn(ex.ToString(), ex);
                throw new UserFriendlyException(L("InvlalidLanguageCode"));
            }
        }
        /// <summary>
        /// 检查已存在的语言
        /// </summary>
        /// <param name="languageName">语言名称</param>
        /// <param name="expectedId">预期ID</param>
        /// <returns></returns>
        private async Task CheckLanguageIfAlreadyExists(string languageName, int? expectedId = null)
        {
            var existingLanguage = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId))
                .FirstOrDefault(l => l.Name == languageName);

            if (existingLanguage == null)
            {
                return;
            }

            if (expectedId != null && existingLanguage.Id == expectedId.Value)
            {
                return;
            }

            throw new UserFriendlyException(L("ThisLanguageAlreadyExists"));
        }
    }
}