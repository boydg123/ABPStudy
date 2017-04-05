using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Notifications
{
    /// <summary>
    /// 通知设置提供者
    /// </summary>
    public class NotificationSettingProvider : SettingProvider
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
                new SettingDefinition(
                    NotificationSettingNames.ReceiveNotifications,
                    "true",
                    L("ReceiveNotifications"),
                    scopes: SettingScopes.User,
                    isVisibleToClients: true)
            };
        }

        /// <summary>
        /// 本地化设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}
