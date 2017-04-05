namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 事件总线配置
    /// </summary>
    internal class EventBusConfiguration : IEventBusConfiguration
    {
        /// <summary>
        /// 是否使用默认事件总线
        /// </summary>
        public bool UseDefaultEventBus { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EventBusConfiguration()
        {
            UseDefaultEventBus = true;
        }
    }
}