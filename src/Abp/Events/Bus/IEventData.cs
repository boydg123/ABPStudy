using System;

namespace Abp.Events.Bus
{
    /// <summary>
    /// Defines interface for all Event data classes.
    /// 为所有的事件数据类定义的接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// The time when the event occured.
        /// 事件发生的时间
        /// </summary>
        DateTime EventTime { get; set; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// 触发事件的对象（可选）
        /// </summary>
        object EventSource { get; set; }
    }
}
