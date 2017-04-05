using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Timing
{
    /// <summary>
    /// 时间设置
    /// </summary>
    public class TimingSettingProvider : SettingProvider
    {
        /// <summary>
        /// 获取设置的一些定义
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(TimingSettingNames.TimeZone, "UTC", L("TimeZone"), scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }

        /// <summary>
        /// 本地化设置
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}
