using Abp.Dependency;
using Abp.Events.Bus;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure <see cref="IEventBus"/>.
    /// 用于配置<see cref="IEventBus"/>.
    /// </summary>
    public interface IEventBusConfiguration
    {
        /// <summary>
        /// True, to use <see cref="EventBus.Default"/>.False, to create per <see cref="IIocManager"/>.
        /// True,使用 <see cref="EventBus.Default"/>.False, to create per <see cref="IIocManager"/>.
        /// This is generally set to true. But, for unit tests,it can be set to false.Default: true.
        /// 一般设置为True,单元测试时设置为false。默认为: true.
        /// </summary>
        bool UseDefaultEventBus { get; set; }
    }
}