using System;

namespace Abp.Timing
{
    /// <summary>
    /// Implements <see cref="IClockProvider"/> to work with local times.
    /// <see cref="IClockProvider"/>于本地时间的实现
    /// </summary>
    public class LocalClockProvider : IClockProvider
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// 本地时间
        /// </summary>
        public DateTimeKind Kind => DateTimeKind.Local;

        /// <summary>
        /// 不支持时间类型
        /// </summary>
        public bool SupportsMultipleTimezone => false;

        /// <summary>
        /// 规范化给定的 <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">要规范化的时间</param>
        /// <returns>规范化的时间</returns>
        public DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }

            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.ToLocalTime();
            }

            return dateTime;
        }

        internal LocalClockProvider()
        {

        }
    }
}