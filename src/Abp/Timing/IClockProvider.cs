using System;

namespace Abp.Timing
{
    /// <summary>
    /// Defines interface to perform some common date-time operations.
    /// 定义执行一些常规的date-time操作的接口
    /// </summary>
    public interface IClockProvider
    {
        /// <summary>
        /// Gets Now.
        /// 获取当前时间
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets kind.
        /// 时间类型
        /// </summary>
        DateTimeKind Kind { get; }

        /// <summary>
        /// Is that provider supports multiple time zone.
        /// 是否支持多时区
        /// </summary>
        bool SupportsMultipleTimezone { get; }

        /// <summary>
        /// Normalizes given <see cref="DateTime"/>.
        /// 规范化给定的 <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized. / 要规范化的时间</param>
        /// <returns>Normalized DateTime / 规范化的时间</returns>
        DateTime Normalize(DateTime dateTime);
    }
}