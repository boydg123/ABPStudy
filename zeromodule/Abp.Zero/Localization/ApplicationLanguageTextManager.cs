using System.Globalization;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;

namespace Abp.Localization
{
    /// <summary>
    /// Manages localization texts for host and tenants.
    /// <see cref="IApplicationLanguageTextManager"/>的实现，管理宿主和租户的本地化文本
    /// </summary>
    public class ApplicationLanguageTextManager : IApplicationLanguageTextManager, ITransientDependency
    {
        /// <summary>
        /// 本地化管理引用
        /// </summary>
        private readonly ILocalizationManager _localizationManager;
        /// <summary>
        /// 应用程序语言文本仓储
        /// </summary>
        private readonly IRepository<ApplicationLanguageText, long> _applicationTextRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationLanguageTextManager(
            ILocalizationManager localizationManager, 
            IRepository<ApplicationLanguageText, long> applicationTextRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _localizationManager = localizationManager;
            _applicationTextRepository = applicationTextRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Gets a localized string value.
        /// 获取一个本地化字符串值
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(如果是宿主商户)</param>
        /// <param name="sourceName">Source name / 源名称</param>
        /// <param name="culture">Culture / 区域文化信息</param>
        /// <param name="key">Localization key / 本地化Key</param>
        /// <param name="tryDefaults">True: fallbacks to default languages if can not find in given culture / True,如果从给定的区域中找不到则返回默认语言</param>
        public string GetStringOrNull(int? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true)
        {
            var source = _localizationManager.GetSource(sourceName);

            if (!(source is IMultiTenantLocalizationSource))
            {
                return source.GetStringOrNull(key, culture, tryDefaults);
            }

            return source
                .As<IMultiTenantLocalizationSource>()
                .GetStringOrNull(tenantId, key, culture, tryDefaults);
        }

        /// <summary>
        /// Updates a localized string value.
        /// 更新一个本地化字符串值
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(如果是宿主商户)</param>
        /// <param name="sourceName">Source name / 源名称</param>
        /// <param name="culture">Culture / 区域文化信息</param>
        /// <param name="key">Localization key / 本地化Key</param>
        /// <param name="value">New localized value. / 新本地化值</param>
        [UnitOfWork]
        public virtual async Task UpdateStringAsync(int? tenantId, string sourceName, CultureInfo culture, string key, string value)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var existingEntity = await _applicationTextRepository.FirstOrDefaultAsync(t =>
                    t.Source == sourceName &&
                    t.LanguageName == culture.Name &&
                    t.Key == key
                    );

                if (existingEntity != null)
                {
                    if (existingEntity.Value != value)
                    {
                        existingEntity.Value = value;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                    }
                }
                else
                {
                    await _applicationTextRepository.InsertAsync(
                        new ApplicationLanguageText
                        {
                           TenantId = tenantId,
                           Source = sourceName,
                           LanguageName = culture.Name,
                           Key = key,
                           Value = value
                        });
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
    }
}