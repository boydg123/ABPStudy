using System;

namespace Abp.Timing
{
    /// <summary>
    /// Used to perform some common date-time operations.
    /// 用于执行一些常规的date-time操作
    /// </summary>
    public static class Clock
    {
        /// <summary>
        /// This object is used to perform all <see cref="Clock"/> operations.Default value: <see cref="UnspecifiedClockProvider"/>.
        /// 这个对象用于执行所有的<see cref="Clock"/>操作.Default value: <see cref="UnspecifiedClockProvider"/>.
        /// </summary>
        public static IClockProvider Provider
        {
            get { return _provider; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Can not set Clock.Provider to null!");
                }

                _provider = value;
            }
        }

        private static IClockProvider _provider;

        /// <summary>
        /// 构造函数
        /// </summary>
        static Clock()
        {
            Provider = ClockProviders.Unspecified;
        }

        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// 使用 <see cref="Provider"/>获取当前时间.
        /// </summary>
        public static DateTime Now => Provider.Now;

        public static DateTimeKind Kind => Provider.Kind;

        /// <summary>
        /// Returns true if multiple timezone is supported, returns false if not.
        /// 是否支持多时区
        /// </summary>
        public static bool SupportsMultipleTimezone => Provider.SupportsMultipleTimezone;

        /// <summary>
        /// Normalizes given <see cref="DateTime"/> using current <see cref="Provider"/>.
        /// 规范化给定的 <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized. / 要规范化的时间</param>
        /// <returns>Normalized DateTime / 规范化的时间</returns>
        public static DateTime Normalize(DateTime dateTime)
        {
            return Provider.Normalize(dateTime);
        }
    }
}