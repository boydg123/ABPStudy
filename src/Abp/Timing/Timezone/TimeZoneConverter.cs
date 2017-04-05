using System;
using Abp.Configuration;
using Abp.Dependency;

namespace Abp.Timing.Timezone
{
    /// <summary>
    /// Time zone converter class
    /// 时区转换类
    /// </summary>
    public class TimeZoneConverter : ITimeZoneConverter, ITransientDependency
    {
        /// <summary>
        /// 设置管理类
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Constructor
        /// 构造函数
        /// </summary>
        /// <param name="settingManager"></param>
        public TimeZoneConverter(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// 将给定的时间转换为用户的时区,如果未指定的时区设置，返回给定日期
        /// </summary>
        /// <param name="date">要转换的时间</param>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public DateTime? Convert(DateTime? date, int? tenantId, long userId)
        {
            if (!date.HasValue)
            {
                return null;
            }

            if (!Clock.SupportsMultipleTimezone)
            {
                return date;
            }

            var usersTimezone = _settingManager.GetSettingValueForUser(TimingSettingNames.TimeZone, tenantId, userId);
            if(string.IsNullOrEmpty(usersTimezone))
            {
                return date;
            }
            
            return TimezoneHelper.ConvertFromUtc(date.Value.ToUniversalTime(), usersTimezone);
        }

        /// <summary>
        /// 将给定的时间转换为租户的时区。如果未指定的时区设置，返回给定日期。
        /// </summary>
        /// <param name="date">要转换的时间</param>
        /// <param name="tenantId">租户ID</param>
        /// <returns></returns>
        public DateTime? Convert(DateTime? date, int tenantId)
        {
            if (!date.HasValue)
            {
                return null;
            }

            if (!Clock.SupportsMultipleTimezone)
            {
                return date;
            }

            var tenantsTimezone = _settingManager.GetSettingValueForTenant(TimingSettingNames.TimeZone, tenantId);
            if (string.IsNullOrEmpty(tenantsTimezone))
            {
                return date;
            }

            return TimezoneHelper.ConvertFromUtc(date.Value.ToUniversalTime(), tenantsTimezone);
        }

        /// <summary>
        /// 将给定的时间转换为应用程序的时区。如果未指定的时区设置，返回给定日期。
        /// </summary>
        /// <param name="date">要转换的时间</param>
        /// <returns></returns>
        public DateTime? Convert(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            if (!Clock.SupportsMultipleTimezone)
            {
                return date;
            }

            var applicationsTimezone = _settingManager.GetSettingValueForApplication(TimingSettingNames.TimeZone);
            if (string.IsNullOrEmpty(applicationsTimezone))
            {
                return date;
            }

            return TimezoneHelper.ConvertFromUtc(date.Value.ToUniversalTime(), applicationsTimezone);
        }
    }
}