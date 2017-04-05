using System;

namespace Abp.Timing
{
    /// <summary>
    /// 未指定时钟驱动
    /// </summary>
    public class UnspecifiedClockProvider : IClockProvider
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// 表示的时间既未指定为本地时间，也未指定为协调通用时间 (UTC)
        /// </summary>
        public DateTimeKind Kind => DateTimeKind.Unspecified;

        /// <summary>
        /// 不支持多时区
        /// </summary>
        public bool SupportsMultipleTimezone => false;

        /// <summary>
        /// 规范化给定的 <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">要规范化的时间</param>
        /// <returns>规范化的时间</returns>
        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        internal UnspecifiedClockProvider()
        {
            
        }
    }
}