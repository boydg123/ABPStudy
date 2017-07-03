namespace Abp.Configuration
{
    /// <summary>
    /// Implements methods to convert objects between SettingInfo and Setting classes.
    /// 实现将设置信息和设置类之间转换的方法
    /// </summary>
    internal static class SettingExtensions
    {
        /// <summary>
        /// 从给定的<see cref="SettingInfo"/>对象创建一个<see cref="Setting"/>对象
        /// </summary>
        public static Setting ToSetting(this SettingInfo settingInfo)
        {
            return settingInfo == null
                ? null
                : new Setting(settingInfo.TenantId, settingInfo.UserId, settingInfo.Name, settingInfo.Value);
        }

        /// <summary>
        /// 从给定的<see cref="Setting"/>对象创建一个<see cref="SettingInfo"/>对象
        /// </summary>
        public static SettingInfo ToSettingInfo(this Setting setting)
        {
            return setting == null
                ? null
                : new SettingInfo(setting.TenantId, setting.UserId, setting.Name, setting.Value);
        }
    }
}