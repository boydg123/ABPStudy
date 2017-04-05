namespace Abp.Configuration
{
    /// <summary>
    /// Represents value of a setting.
    /// 表示一个设置值
    /// </summary>
    public interface ISettingValue
    {
        /// <summary>
        /// Unique name of the setting.
        /// 设置的唯一性名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value of the setting.
        /// 设置的值
        /// </summary>
        string Value { get; }
    }
}