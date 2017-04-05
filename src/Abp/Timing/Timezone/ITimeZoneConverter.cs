using System;

namespace Abp.Timing.Timezone
{
    /// <summary>
    /// Interface for timezone converter
    /// 时区转换器接口
    /// </summary>
    public interface ITimeZoneConverter
    {
        /// <summary>
        /// Converts given date to user's time zone. 
        /// 将给定的时间转换为用户的时区
        /// If timezone setting is not specified, returns given date.
        /// 如果未指定的时区设置，返回给定日期
        /// </summary>
        /// <param name="date">Base date to convert / 要转换的时间</param>
        /// <param name="tenantId">TenantId of user / 租户ID</param>
        /// <param name="userId">UserId to convert date for / 用户ID</param>
        /// <returns></returns>
        DateTime? Convert(DateTime? date, int? tenantId, long userId);

        /// <summary>
        /// Converts given date to tenant's time zone. If timezone setting is not specified, returns given date.
        /// 将给定的时间转换为租户的时区。如果未指定的时区设置，返回给定日期。
        /// </summary>
        /// <param name="date">Base date to convert / 要转换的时间</param>
        /// <param name="tenantId">TenantId  to convert date for / 租户ID</param>
        /// <returns></returns>
        DateTime? Convert(DateTime? date, int tenantId);

        /// <summary>
        /// Converts given date to application's time zone. If timezone setting is not specified, returns given date.
        /// 将给定的时间转换为应用程序的时区。如果未指定的时区设置，返回给定日期。
        /// </summary>
        /// <param name="date">Base date to convert / 要转换的时间</param>
        /// <returns></returns>
        DateTime? Convert(DateTime? date);
    }
}
