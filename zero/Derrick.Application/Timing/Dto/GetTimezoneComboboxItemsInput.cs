using Abp.Configuration;

namespace Derrick.Timing.Dto
{
    /// <summary>
    /// 获取时区Combobox项Input
    /// </summary>
    public class GetTimezoneComboboxItemsInput
    {
        /// <summary>
        /// 默认时区区域
        /// </summary>
        public SettingScopes DefaultTimezoneScope;
        /// <summary>
        /// 选中的时区ID
        /// </summary>
        public string SelectedTimezoneId { get; set; }
    }
}
