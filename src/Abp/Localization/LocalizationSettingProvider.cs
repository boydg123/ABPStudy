using System.Collections.Generic;
using Abp.Configuration;

namespace Abp.Localization
{
    /// <summary>
    /// 本地化设置提供者
    /// </summary>
    public class LocalizationSettingProvider : SettingProvider
    {
        /// <summary>
        /// 获取设置定义
        /// </summary>
        /// <param name="context">设置定义提供者上下文</param>
        /// <returns></returns>
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(LocalizationSettingNames.DefaultLanguage, null, L("DefaultLanguage"), scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }

        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}