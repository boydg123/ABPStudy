using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Test.Events.Bus
{
    /// <summary>
    /// 实时事件处理器
    /// </summary>
    public class MySimpleTransientEventHandler : IEventHandler<MySimpleEventData>, IDisposable
    {
        /// <summary>
        /// 处理次数
        /// </summary>
        public static int HandleCount { get; set; }

        /// <summary>
        /// 释放次数
        /// </summary>
        public static int DisposeCount { get; set; }
        public void Dispose()
        {
            ++DisposeCount;
        }

        public void HandleEvent(MySimpleEventData eventData)
        {
            ++HandleCount;
        }
    }
}
