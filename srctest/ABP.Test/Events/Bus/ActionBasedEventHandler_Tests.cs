using Xunit;
using Shouldly;
using System;

namespace Abp.Test.Events.Bus
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionBasedEventHandler_Tests : EventBusTestBase
    {
        /// <summary>
        /// 应以正确的Source在事件上调用方法
        /// </summary>
        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalCount = 0;
            EventBus.Register<MySimpleEventData>(eventData =>
            {
                totalCount += eventData.Value;
                //当前对象就是事件源
                Assert.Equal(this, eventData.EventSource);
            });

            EventBus.Trigger(this, new MySimpleEventData(1));
            EventBus.Trigger(this, new MySimpleEventData(2));
            EventBus.Trigger(this, new MySimpleEventData(3));
            EventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(10, totalCount);
        }

        /// <summary>
        /// 用非通用触发器调用处理器
        /// </summary>
        [Fact]
        public void Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;
            EventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });
            EventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(1));
            EventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(2));
            EventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(3));
            EventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        /// <summary>
        /// 在注册器释放后不会调用触发器方法 - 1
        /// </summary>
        [Fact]
        public void Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            var registerDisposer = EventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
            });

            EventBus.Trigger(this, new MySimpleEventData(1));
            EventBus.Trigger(this, new MySimpleEventData(2));
            EventBus.Trigger(this, new MySimpleEventData(3));

            registerDisposer.Dispose();

            EventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }

        /// <summary>
        /// 在注册器释放后不会调用触发器方法 - 2
        /// </summary>
        [Fact]
        public void Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Action<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
            });

            EventBus.Register(action);

            EventBus.Trigger(this, new MySimpleEventData(1));
            EventBus.Trigger(this, new MySimpleEventData(2));
            EventBus.Trigger(this, new MySimpleEventData(3));

            EventBus.Unregister(action);

            EventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }
    }
}
