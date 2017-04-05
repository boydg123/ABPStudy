using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Net.Mail
{
    /// <summary>
    /// Defines settings to send emails.<see cref="EmailSettingNames"/> for all available configurations.
    /// 定义发送邮件的设置,<see cref="EmailSettingNames"/>为所有可用的配置
    /// </summary>
    internal class EmailSettingProvider : SettingProvider
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
                       new SettingDefinition(EmailSettingNames.Smtp.Host, "127.0.0.1", L("SmtpHost"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Port, "25", L("SmtpPort"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.UserName, "", L("Username"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Password, "", L("Password"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Domain, "", L("DomainName"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false", L("UseSSL"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true", L("UseDefaultCredentials"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.DefaultFromAddress, "", L("DefaultFromSenderEmailAddress"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "", L("DefaultFromSenderDisplayName"), scopes: SettingScopes.Application | SettingScopes.Tenant)
                   };
        }

        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}