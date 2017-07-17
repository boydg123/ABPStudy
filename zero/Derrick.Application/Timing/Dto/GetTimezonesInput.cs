using Abp.Configuration;

namespace Derrick.Timing.Dto
{
    /// <summary>
    /// 获取时区Input
    /// </summary>
    public class GetTimezonesInput
    {
        /// <summary>
        /// 默认时区范围
        /// </summary>
        public SettingScopes DefaultTimezoneScope;
    }
}
