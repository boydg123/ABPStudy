using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Timing;
using Abp.Extensions;

namespace Derrick.Timing
{
    /// <summary>
    /// <see cref="ITimeZoneService"/>时区服务实现。
    /// </summary>
    public class TimeZoneService : ITimeZoneService, ITransientDependency
    {
        /// <summary>
        /// 设置管理器
        /// </summary>
        readonly ISettingManager _settingManager;
        /// <summary>
        /// 设置定义管理器
        /// </summary>
        readonly ISettingDefinitionManager _settingDefinitionManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settingManager">设置管理器</param>
        /// <param name="settingDefinitionManager">设置定义管理器</param>
        public TimeZoneService(
            ISettingManager settingManager, 
            ISettingDefinitionManager settingDefinitionManager)
        {
            _settingManager = settingManager;
            _settingDefinitionManager = settingDefinitionManager;
        }

        /// <summary>
        /// 获取默认时区
        /// </summary>
        /// <param name="scope">设置作用域</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        public async Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId)
        {
            if (scope == SettingScopes.User)
            {
                if (tenantId.HasValue)
                {
                    return await _settingManager.GetSettingValueForTenantAsync(TimingSettingNames.TimeZone, tenantId.Value);
                }

                return await _settingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            }

            if (scope == SettingScopes.Tenant)
            {
                return await _settingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            }

            if (scope == SettingScopes.Application)
            {
                var timezoneSettingDefinition = _settingDefinitionManager.GetSettingDefinition(TimingSettingNames.TimeZone);
                return timezoneSettingDefinition.DefaultValue;
            }

            throw new Exception("Unknown scope for default timezone setting.");
        }
    }
}