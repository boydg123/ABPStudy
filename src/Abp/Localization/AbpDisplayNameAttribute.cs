using System.ComponentModel;

namespace Abp.Localization
{
    /// <summary>
    /// ABP显示名称特性
    /// </summary>
    public class AbpDisplayNameAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// 获取DisplayName
        /// </summary>
        public override string DisplayName => LocalizationHelper.GetString(SourceName, Key);

        /// <summary>
        /// 源名称
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 键名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceName">源名称</param>
        /// <param name="key">键名称</param>
        public AbpDisplayNameAttribute(string sourceName, string key)
        {
            SourceName = sourceName;
            Key = key;
        }
    }
}
