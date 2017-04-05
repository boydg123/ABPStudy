using System;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DayOfWeekExtensions"/>.
    /// <see cref="DayOfWeekExtensions"/>.扩展方法
    /// </summary>
    public static class DayOfWeekExtensions
    {
        /// <summary>
        /// Check if given <see cref="DayOfWeek"/> value is weekend.
        /// 检查给定的<see cref="DayOfWeek"/> 值是否是周未
        /// </summary>
        public static bool IsWeekend(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Check if given <see cref="DayOfWeek"/> value is weekday.
        /// 检查给定的<see cref="DayOfWeek"/>的值是否是工作日
        /// </summary>
        public static bool IsWeekday(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek.IsIn(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday);
        }
    }
}