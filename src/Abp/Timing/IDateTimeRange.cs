using System;

namespace Abp.Timing
{
    /// <summary>
    /// Defines interface for a DateTime range.
    /// 定义时间范围的接口
    /// </summary>
    public interface IDateTimeRange
    {
        /// <summary>
        /// Start time of the datetime range.
        /// 开始时间
        /// </summary>
        DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the datetime range.
        /// 结束时间
        /// </summary>
        DateTime EndTime { get; set; }
    }
}
