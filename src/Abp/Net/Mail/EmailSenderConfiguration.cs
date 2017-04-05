using System;
using Abp.Configuration;
using Abp.Extensions;

namespace Abp.Net.Mail
{
    /// <summary>
    /// Implementation of <see cref="IEmailSenderConfiguration"/> that reads settings from <see cref="ISettingManager"/>.
    /// 实现接口<see cref="IEmailSenderConfiguration"/>，从<see cref="ISettingManager"/>读取配置.
    /// </summary>
    public abstract class EmailSenderConfiguration : IEmailSenderConfiguration
    {
        /// <summary>
        /// 默认from地址
        /// </summary>
        public string DefaultFromAddress
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.DefaultFromAddress); }
        }

        /// <summary>
        /// 默认显示名称
        /// </summary>
        public string DefaultFromDisplayName
        {
            get { return SettingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName); }
        }

        /// <summary>
        /// 设置管理器
        /// </summary>
        protected readonly ISettingManager SettingManager;

        /// <summary>
        /// Creates a new <see cref="EmailSenderConfiguration"/>.
        /// 构造函数
        /// </summary>
        protected EmailSenderConfiguration(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        /// <summary>
        /// Gets a setting value by checking. Throws <see cref="AbpException"/> if it's null or empty.
        /// 获取一个通过了检查的设置值，如果为null或者为空，将抛出异常<see cref="AbpException"/> 
        /// </summary>
        /// <param name="name">Name of the setting / 设置名称</param>
        /// <returns>Value of the setting / 设置值</returns>
        protected string GetNotEmptySettingValue(string name)
        {
            var value = SettingManager.GetSettingValue(name);
            if (value.IsNullOrEmpty())
            {
                throw new AbpException(String.Format("Setting value for '{0}' is null or empty!", name));
            }

            return value;
        }
    }
}