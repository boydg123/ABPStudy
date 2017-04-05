using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Test.Events.Bus
{
    /// <summary>
    /// 事件总线测试基类
    /// </summary>
    public abstract class EventBusTestBase
    {
        protected IEventBus EventBus;
        protected EventBusTestBase()
        {
            EventBus = new EventBus();
        }
    }
}
