using System;

namespace Abp.Timing
{
    /// <summary>
    /// A basic implementation of <see cref="IDateTimeRange"/> to store a date range.
    /// <see cref="IDateTimeRange"/>的基本实现，用以存储一个时间范围.
    /// </summary>
    [Serializable]
    public class DateTimeRange : IDateTimeRange
    {
        /// <summary>
        /// Start time of the datetime range.
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the datetime range.
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        private static DateTime Now { get { return Clock.Now; } }

        /// <summary>
        /// Creates a new <see cref="DateTimeRange"/> object.
        /// 创建一个新的 <see cref="DateTimeRange"/> 对象.
        /// </summary>
        public DateTimeRange()
        {

        }

        /// <summary>
        /// Creates a new <see cref="DateTimeRange"/> object from given <paramref name="startTime"/> and <paramref name="endTime"/>.
        /// 使用给定的 <paramref name="startTime"/> 和 <paramref name="endTime"/>，创建一个新的 <see cref="DateTimeRange"/> 对象 .
        /// </summary>
        /// <param name="startTime">Start time of the datetime range / 开始时间</param>
        /// <param name="endTime">End time of the datetime range / 结束时间</param>
        public DateTimeRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Creates a new <see cref="DateTimeRange"/> object from given <paramref name="dateTimeRange"/> object.
        /// 使用给定的 <paramref name="dateTimeRange"/>，创建一个新的 <see cref="DateTimeRange"/> 对象 .
        /// </summary>
        /// <param name="dateTimeRange">IDateTimeRange object / IDateTimeRange 对象</param>
        public DateTimeRange(IDateTimeRange dateTimeRange)
        {
            StartTime = dateTimeRange.StartTime;
            EndTime = dateTimeRange.EndTime;
        }

        /// <summary>
        /// Gets a date range represents yesterday.
        /// 获取一个表示昨天的日期范围
        /// </summary>
        public static DateTimeRange Yesterday
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents today.
        /// 获取一个表示今天的日期范围
        /// </summary>
        public static DateTimeRange Today
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.Date, now.Date.AddDays(1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents tomorrow.
        /// 获取一个表示明天的日期范围
        /// </summary>
        public static DateTimeRange Tomorrow
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.Date.AddDays(1), now.Date.AddDays(2).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents the last month.
        /// 获取一个表示最近一个月的日期范围
        /// </summary>
        public static DateTimeRange LastMonth
        {
            get
            {
                var now = Now;
                var startTime = now.Date.AddDays(-now.Day + 1).AddMonths(-1);
                var endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }

        /// <summary>
        /// Gets a date range represents this month.
        /// 获取一个表示本月的日期范围
        /// </summary>
        public static DateTimeRange ThisMonth
        {
            get
            {
                var now = Now;
                var startTime = now.Date.AddDays(-now.Day + 1);
                var endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }

        /// <summary>
        /// Gets a date range represents the next month.
        /// 获取一个表示下月的日期范围
        /// </summary>
        public static DateTimeRange NextMonth
        {
            get
            {
                var now = Now;
                var startTime = now.Date.AddDays(-now.Day + 1).AddMonths(1);
                var endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }


        /// <summary>
        /// Gets a date range represents the last year.
        /// 获取一个表示上一年的日期范围
        /// </summary>
        public static DateTimeRange LastYear
        {
            get
            {
                var now = Now;
                return new DateTimeRange(new DateTime(now.Year - 1, 1, 1), new DateTime(now.Year, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents this year.
        /// 获取一个表示本年的日期范围
        /// </summary>
        public static DateTimeRange ThisYear
        {
            get
            {
                var now = Now;
                return new DateTimeRange(new DateTime(now.Year, 1, 1), new DateTime(now.Year + 1, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents the next year.
        /// 获取一个表示下一年的日期范围
        /// </summary>
        public static DateTimeRange NextYear
        {
            get
            {
                var now = Now;
                return new DateTimeRange(new DateTime(now.Year + 1, 1, 1), new DateTime(now.Year + 2, 1, 1).AddMilliseconds(-1));
            }
        }


        /// <summary>
        /// Gets a date range represents the last 30 days (30x24 hours) including today.
        /// 获取一个表示上一个30天（30x24,包含今天）的日期范围
        /// </summary>
        public static DateTimeRange Last30Days
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.AddDays(-30), now);
            }
        }

        /// <summary>
        /// Gets a date range represents the last 30 days excluding today.
        /// 获取一个表示上一个30天（不包含今天）的日期范围
        /// </summary>
        public static DateTimeRange Last30DaysExceptToday
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.Date.AddDays(-30), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Gets a date range represents the last 7 days (7x24 hours) including today.
        /// 获取一个表示上一个7天（7x24,包含今天）的日期范围
        /// </summary>
        public static DateTimeRange Last7Days
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.AddDays(-7), now);
            }
        }

        /// <summary>
        /// Gets a date range represents the last 7 days excluding today.
        /// 获取一个表示上一个7天（7x24）的日期范围.
        /// </summary>
        public static DateTimeRange Last7DaysExceptToday
        {
            get
            {
                var now = Now;
                return new DateTimeRange(now.Date.AddDays(-7), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="DateTimeRange"/>.
        /// 返回一个表示当前<see cref="DateTimeRange"/>的<see cref="System.String"/> .
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="DateTimeRange"/>. / 一个表示当前<see cref="DateTimeRange"/>的<see cref="System.String"/>.</returns>
        public override string ToString()
        {
            return string.Format("[{0} - {1}]", StartTime, EndTime);
        }
    }
}