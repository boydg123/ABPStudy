using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Abp.Web.Models.AbpUserConfiguration;
using Abp.Web.Security.AntiForgery;
using System.Linq;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Web.Configuration
{
    /// <summary>
    /// ABP用户配置构建器
    /// </summary>
    public class AbpUserConfigurationBuilder : ITransientDependency
    {
        /// <summary>
        /// 多租户配置
        /// </summary>
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        /// <summary>
        /// 语言管理器
        /// </summary>
        private readonly ILanguageManager _languageManager;

        /// <summary>
        /// 本地化管理器
        /// </summary>
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// 功能管理器
        /// </summary>
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// 功能检查器
        /// </summary>
        private readonly IFeatureChecker _featureChecker;

        /// <summary>
        /// 权限管理器
        /// </summary>
        private readonly IPermissionManager _permissionManager;

        /// <summary>
        /// 用户导航管理器
        /// </summary>
        private readonly IUserNavigationManager _userNavigationManager;

        /// <summary>
        /// 设置定义管理器
        /// </summary>
        private readonly ISettingDefinitionManager _settingDefinitionManager;

        /// <summary>
        /// 设置管理器
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// ABP防伪配置
        /// </summary>
        private readonly IAbpAntiForgeryConfiguration _abpAntiForgeryConfiguration;

        /// <summary>
        /// ABP Session
        /// </summary>
        private readonly IAbpSession _abpSession;

        /// <summary>
        /// 权限检查器
        /// </summary>
        private readonly IPermissionChecker _permissionChecker;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancyConfig">多租户配置</param>
        /// <param name="languageManager">语言管理器</param>
        /// <param name="localizationManager">本地化管理器</param>
        /// <param name="featureManager">功能管理器</param>
        /// <param name="featureChecker">功能检查器</param>
        /// <param name="permissionManager">权限管理器</param>
        /// <param name="userNavigationManager">用户导航管理器</param>
        /// <param name="settingDefinitionManager">设置定义管理器</param>
        /// <param name="settingManager">设置管理器</param>
        /// <param name="abpAntiForgeryConfiguration">ABP防伪配置</param>
        /// <param name="abpSession">ABP Session</param>
        /// <param name="permissionChecker">权限检查器</param>
        public AbpUserConfigurationBuilder(
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager,
            ILocalizationManager localizationManager,
            IFeatureManager featureManager,
            IFeatureChecker featureChecker,
            IPermissionManager permissionManager,
            IUserNavigationManager userNavigationManager,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IAbpAntiForgeryConfiguration abpAntiForgeryConfiguration,
            IAbpSession abpSession,
            IPermissionChecker permissionChecker)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
            _localizationManager = localizationManager;
            _featureManager = featureManager;
            _featureChecker = featureChecker;
            _permissionManager = permissionManager;
            _userNavigationManager = userNavigationManager;
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _abpAntiForgeryConfiguration = abpAntiForgeryConfiguration;
            _abpSession = abpSession;
            _permissionChecker = permissionChecker;
        }
        
        /// <summary>
        /// 获取用户所有配置
        /// </summary>
        /// <returns></returns>
        public async Task<AbpUserConfigurationDto> GetAll()
        {
            return new AbpUserConfigurationDto
            {
                MultiTenancy = GetUserMultiTenancyConfig(),
                Session = GetUserSessionConfig(),
                Localization = GetUserLocalizationConfig(),
                Features = await GetUserFeaturesConfig(),
                Auth = await GetUserAuthConfig(),
                Nav = await GetUserNavConfig(),
                Setting = await GetUserSettingConfig(),
                Clock = GetUserClockConfig(),
                Timing = await GetUserTimingConfig(),
                Security = GetUserSecurityConfig()
            };
        }

        /// <summary>
        /// 获取用户多租户配置
        /// </summary>
        /// <returns></returns>
        private AbpMultiTenancyConfigDto GetUserMultiTenancyConfig()
        {
            return new AbpMultiTenancyConfigDto
            {
                IsEnabled = _multiTenancyConfig.IsEnabled
            };
        }

        /// <summary>
        /// 获取用户Session配置
        /// </summary>
        /// <returns></returns>
        private AbpUserSessionConfigDto GetUserSessionConfig()
        {
            return new AbpUserSessionConfigDto
            {
                UserId = _abpSession.UserId,
                TenantId = _abpSession.TenantId,
                ImpersonatorUserId = _abpSession.ImpersonatorUserId,
                ImpersonatorTenantId = _abpSession.ImpersonatorTenantId,
                MultiTenancySide = _abpSession.MultiTenancySide
            };
        }

        /// <summary>
        /// 获取用户本地化配置
        /// </summary>
        /// <returns></returns>
        private AbpUserLocalizationConfigDto GetUserLocalizationConfig()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            var languages = _languageManager.GetLanguages();

            var config = new AbpUserLocalizationConfigDto
            {
                CurrentCulture = new AbpUserCurrentCultureConfigDto
                {
                    Name = currentCulture.Name,
                    DisplayName = currentCulture.DisplayName
                },
                Languages = languages.ToList()
            };

            if (languages.Count > 0)
            {
                config.CurrentLanguage = _languageManager.CurrentLanguage;
            }

            var sources = _localizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();
            config.Sources = sources.Select(s => new AbpLocalizationSourceDto
            {
                Name = s.Name,
                Type = s.GetType().Name
            }).ToList();

            config.Values = new Dictionary<string, Dictionary<string, string>>();
            foreach (var source in sources)
            {
                var stringValues = source.GetAllStrings(currentCulture).OrderBy(s => s.Name).ToList();
                var stringDictionary = stringValues
                    .ToDictionary(_ => _.Name, _ => _.Value);
                config.Values.Add(source.Name, stringDictionary);
            }

            return config;
        }

        /// <summary>
        /// 获取用户功能配置
        /// </summary>
        /// <returns></returns>
        private async Task<AbpUserFeatureConfigDto> GetUserFeaturesConfig()
        {
            var config = new AbpUserFeatureConfigDto()
            {
                AllFeatures = new Dictionary<string, AbpStringValueDto>()
            };

            var allFeatures = _featureManager.GetAll().ToList();

            if (_abpSession.TenantId.HasValue)
            {
                var currentTenantId = _abpSession.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    var value = await _featureChecker.GetValueAsync(currentTenantId, feature.Name);
                    config.AllFeatures.Add(feature.Name, new AbpStringValueDto
                    {
                        Value = value
                    });
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    config.AllFeatures.Add(feature.Name, new AbpStringValueDto
                    {
                        Value = feature.DefaultValue
                    });
                }
            }

            return config;
        }

        /// <summary>
        /// 获取用户认证配置
        /// </summary>
        /// <returns></returns>
        private async Task<AbpUserAuthConfigDto> GetUserAuthConfig()
        {
            var config = new AbpUserAuthConfigDto();

            var allPermissionNames = _permissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            var grantedPermissionNames = new List<string>();

            if (_abpSession.UserId.HasValue)
            {
                foreach (var permissionName in allPermissionNames)
                {
                    if (await _permissionChecker.IsGrantedAsync(permissionName))
                    {
                        grantedPermissionNames.Add(permissionName);
                    }
                }
            }

            config.AllPermissions = allPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");
            config.GrantedPermissions = grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            return config;
        }

        /// <summary>
        /// 获取用户导航配置
        /// </summary>
        /// <returns></returns>
        private async Task<AbpUserNavConfigDto> GetUserNavConfig()
        {
            var userMenus = await _userNavigationManager.GetMenusAsync(_abpSession.ToUserIdentifier());
            return new AbpUserNavConfigDto
            {
                Menus = userMenus.ToDictionary(userMenu => userMenu.Name, userMenu => userMenu)
            };
        }

        /// <summary>
        /// 获取用户设置配置
        /// </summary>
        /// <returns></returns>
        private async Task<AbpUserSettingConfigDto> GetUserSettingConfig()
        {
            var config = new AbpUserSettingConfigDto
            {
                Values = new Dictionary<string, string>()
            };

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.IsVisibleToClients);

            foreach (var settingDefinition in settingDefinitions)
            {
                var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);
                config.Values.Add(settingDefinition.Name, settingValue);
            }

            return config;
        }

        /// <summary>
        /// 获取用户时钟配置
        /// </summary>
        /// <returns></returns>
        private AbpUserClockConfigDto GetUserClockConfig()
        {
            return new AbpUserClockConfigDto
            {
                Provider = Clock.Provider.GetType().Name.ToCamelCase()
            };
        }

        /// <summary>
        /// 获取用户定时配置
        /// </summary>
        /// <returns></returns>
        private async Task<AbpUserTimingConfigDto> GetUserTimingConfig()
        {
            var timezoneId = await _settingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

            return new AbpUserTimingConfigDto
            {
                TimeZoneInfo = new AbpUserTimeZoneConfigDto
                {
                    Windows = new AbpUserWindowsTimeZoneConfigDto
                    {
                        TimeZoneId = timezoneId,
                        BaseUtcOffsetInMilliseconds = timezone.BaseUtcOffset.TotalMilliseconds,
                        CurrentUtcOffsetInMilliseconds = timezone.GetUtcOffset(Clock.Now).TotalMilliseconds,
                        IsDaylightSavingTimeNow = timezone.IsDaylightSavingTime(Clock.Now)
                    },
                    Iana = new AbpUserIanaTimeZoneConfigDto
                    {
                        TimeZoneId = TimezoneHelper.WindowsToIana(timezoneId)
                    }
                }
            };
        }

        /// <summary>
        /// 获取用户安全配置
        /// </summary>
        /// <returns></returns>
        private AbpUserSecurityConfigDto GetUserSecurityConfig()
        {
            return new AbpUserSecurityConfigDto()
            {
                AntiForgery = new AbpUserAntiForgeryConfigDto
                {
                    TokenCookieName = _abpAntiForgeryConfiguration.TokenCookieName,
                    TokenHeaderName = _abpAntiForgeryConfiguration.TokenHeaderName
                }
            };
        }
    }
}