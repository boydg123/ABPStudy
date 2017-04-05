using Abp.Timing;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IDateTimeRange"/>.
    /// <see cref="IDateTimeRange"/>扩展方法.
    /// </summary>
    public static class DateTimeRangeExtensions
    {
        /// <summary>
        /// Sets date range to given target.
        /// 为目标日期范围设置范围
        /// </summary>
        /// <param name="source">源日期</param>
        /// <param name="target">目标日期</param>
        public static void SetTo(this IDateTimeRange source, IDateTimeRange target)
        {
            target.StartTime = source.StartTime;
            target.EndTime = source.EndTime;
        }

        /// <summary>
        /// Sets date range from given source.
        /// 从给定的日期范围设置日期范围
        /// </summary>
        public static void SetFrom(this IDateTimeRange target, IDateTimeRange source)
        {
            target.StartTime = source.StartTime;
            target.EndTime = source.EndTime;
        }
    }
}